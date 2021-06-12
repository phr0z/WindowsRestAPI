# WindowsRestAPI

A Windows Rest API app which allows you to control certain functions of Windows from any browser or rest capable device/service.

## Available features.
- Switch audio output interfaces.
- Adjust audio volume.
- Get active application/window name.
- Switch primary monitor.
- List running processes.
- Create list of executables and call and/or terminate them.

## Known issues.
- Not every exception is handled. Errors will occur and pop up.

#### Installation guide.
Add the program certificate to your local machine root certificate trust. This is needed for Windows Notification Center.

Run "mmc" and add the certificate management snap-in. Select local computer.
Import the certificate into the trusted root certificate store.
Run the installer Package_x.x.x.x_x64_.bundle

Note: commands.config is now stored in %ProgramData% instead. (Copy it from previous location to restore it.)

After installation you need to configure which IP and port number the program binds on.
Default is localhost and 1234 which is allowed by any regular user. But it won't allow you to access it from outside of your own computer.

In order to bind to any other network interface/ip you need to have administrator privileges.
However it is not nice to elevate your priveleges everytime you run this program and pretty soon the UAC prompt will get on your nerves.

Instead we will configure a namespace reservation with the help of net sh. (note that it needs to be run in a elevated command prompt)

`$ netsh http add urlacl url=http://+:1234/ user=DOMAIN\user`

For HTTPS the following is needed.

`$ netsh http add urlacl url=https://+:8443/ user=DOMAIN\user`
`$ netsh http add sslcert ipport=0.0.0.0:8443 certhash=CERTIFICATETHUMBPRINT appid={1534ae42-9336-42f6-a6e8-350428d6f3c2}`

Sidenote; Any certificate can be used. But you need to have the private key! (ie pfx exported with the key, so the included certificate will not work)


Change user to your windows username. In case your running in a local environment, discard DOMAIN\
List the ACL to see if it was added properly.

`$ netsh http show urlacl`


Once the reservation is set go to settings and change IP to  "+" and any port that you decided to reserve (1234 in example case).


#### Endpoints.
- http://localhost:1234/activewindow : Gets the current active window title.
- http://localhost:1234/getid : Gets ID of all active audio interfaces available. Use this ID to switch device.
- http://localhost:1234/getprocesses : Returns a list with current running processes and their runtime.
- http://localhost:1234/set?id={xxxxxxx}.{xxxxxx-xxxx-xxxx-xxx-xxxxxxxxx} : See above.
- http://localhost:1234/set?id=audio%20device%20(name) : Try to set interface by name.
- http://localhost:123/display?primary=(1-4) : Sets your primary display as identified by screen settings. (4).
- http://localhost:1234/run?name=CMD : Runs the defined command as listed by name in settings.
- http://localhost:1234/stop?name=CMD : Tries to terminate the defined command as listed by name in settings. (by process)
- http://localhost:1234/volume?level=55,5 : Sets the volume level for the active audio interface.

#### Home Assistant.
Was one of the main reasons why i developed this program. So I could switch my primary devices when I for example wanted to play Steam on my living room TV.

#### Rest Endpoints in configuration.yaml.
```rest_command:
  monitor_pc:
    url: "http://yourip:1234/display?primary=1"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
  monitor_tv:
    url: "http://yourip:1234/display?primary=2"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
  audio_pc:
    url: "http://yourip:1234/set?id={0.0.0.00000000}.{xxxxxx-xxxx-xxxx-xxx-xxxxxxxxx}"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
  audio_tv:
    url: "http://yourip:1234/set?id={0.0.0.00000000}.{xxxxxx-xxxx-xxxx-xxx-xxxxxxxxx}"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
  steambigpicture:
    url: "http://yourip:1234/run?name=steambigpicture"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
  kodi:
    url: "http://yourip:1234/run?name=kodi"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
```

#### script.yaml example.
```play_steam_on_livingroom_tv:
  sequence:
  - data:
      command:
      - PowerOn
      - OK
      delay_secs: 6
      device: 50285248
      entity_id: remote.myremote
    service: remote.send_command
  - data:
      entity_id: media_player.avr
    service: media_player.turn_on
  - delay: 00:00:05
  - data:
      entity_id: media_player.avr
      source: PC
    service: media_player.select_source
  - delay: 00:00:05
  - service: rest_command.monitor_tv
  - delay: 00:00:05
  - service: rest_command.audio_tv
  - service: rest_command.steambigpicture
  ```

note: See what works in your setup, switching delays might vary depending on equipment!

#### Command example #1: Shutdown computer.
```
Name it: ShutdownComputer
Browse for: C:\Windows\System32\cmd.exe as command.
Add arguments: /c shutdown /s
Run: http://yourip:1234/run?name=ShutdownComputer
```

#### Command example #2: Run powershell script. (use -NoExit to troubleshoot)
```
Browse for: cmd.exe 
Add arguments:/c powershell -NoExit Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted
Browse for: cmd.exe 
Add arguments:/c powershell -NoExit -File C:\Temp\HelloWorld.ps1
```

#### Command example #3: Run Windows Linux Subsystem (WSL) commands.
```
Browse for: cmd.exe 
Add arguments:bash -c netstat -na
```

### Get current display window
```
http://yourip:1234/activewindow
```

### Send a simple message
```
http://yourip:1234/sendmessage?message=HelloWorld
```

### Configuring dynamic notifications from HA using input_text helper configuration.yaml example
```
rest_command:
  send_message:
    url: "http://yourip:1234/sendmessage?message={{ states('input_text.windowsrestapi') }}"
    method: GET
    headers: 
      accept: 'application/json, text/html'
    content_type: 'application/json; charset=utf-8'
    verify_ssl: false
```

### A word on security.
Some might see this program as a major security risk and while that might be true in some cases users should be aware of their environment.
For instance, **you should NEVER expose this program towards internet.** Only use it internally within your own local network.
If neccessary create firewall rules between your client device and Windows computer running this program to explicitly only allow traffic between these two devices.

#### User agreement & disclaimer.
Author of this program is in no form responsible of what might be the result of using this program.
End user is solemnly responsible for what they choose to do with this program and author will not be held responsible.

#### Third party.
- NewtonSoft Json: https://github.com/JamesNK/Newtonsoft.Json
- Grapevine: https://github.com/sukona/Grapevine
- Home Assistant: https://github.com/home-assistant/home-assistant

Thanks to these awesome developers!



#### Buy me a beer (or visit 3rd party devs and buy them a beer!)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=3M28CHQTFECVL&currency_code=SEK&source=url)
