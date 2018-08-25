See https://github.com/amoerie/teamcity-theatre for more info about TeamCity Theatre.

The original version is extended with 
- Docker support
- User settings (images, names)
- Some optimizations
- Display branches
![TC](https://raw.githubusercontent.com/dmytro-pryvedeniuk/teamcity-theatre/5e42fc7cc4fd2ede5031151df8dbe22ab3020a97/TC.png)
## Install Docker for Windows

 Depending on your system setup it can be https://docs.docker.com/toolbox/toolbox_install_windows/ or https://docs.docker.com/docker-for-windows/
 (This solution has been developed using the legacy Docker Toolbox and has not been tried with Docker for Windows).
 With Docker Toolbox install run Docker commands in the "Docker Quickstart Terminal". 
 Ensure that the current directory in your terminal is the directory where *dockerfile* is located, e.g.
```
 cd 'e:\src\teamcitytheatre'
```

## STEP 1 Build the image

It should be done each time when the source code is changed and the changes should be propagated to the docker image.
Run the following to create *teamcitytheatre* Docker image:
```
docker build -t teamcitytheatre .
```
Initially it can take several minutes but next calls thanks to Docker's caching system should be much faster.

## STEP 2 Prepare configuration

Before starting the container some configuration is required

a) Configure your TC server in *docker-compose.yml* file, e.g.
```
    environment:
    - Connection:Url=https://teamcity.jetbrains.com
    - Connection:Username=guest
    - Connection:Password=
```

b) FYI Pay attention to the volumes in *docker-compose.yml* file 
```
    volumes:
    - $HOME/teamcitytheatre:/app/config
    - $HOME/teamcitytheatre/users:/app/wwwroot/users
```
The volumes above are used to map the host's $HOME directory (on Windows it's C:\users\<current user>\) so it will be available in the Docker guest system. 
It allows to stop/remove the containers but keep the configuration alive.
```
    - Storage:ConfigurationFile=/app/config/configuration.json
    - Serilog:1:Args:path=/app/config/logs/TeamCityTheatre.Web-.log 
```

c) User images should be placed to $HOME/teamcitytheatre/users directory (e.g. c:\users\<current user>\teamcitytheatre\users) on the host system. 

d) Having the images in the *users* directory (again, c:\users\<current user>\teamcitytheatre\users) allows to use the relative mapping in *configuration.json* file. 
Also you can specify the complete URL to the image as shown below.

```
... 
  "UserSettings": {
    "Users": [
      {
        "Name": "user1",
        "ImageUrl": "http://www.networkfp.com/wp-content/uploads/2016/08/man-1.jpg"
      },
      {
        "Name": "matt2",
        "ImageUrl": "/users/matt2.jpg"
      }
    ],
    "NullImageUrl": "/users/hamster.jpg"
  }
}
```

e) *docker-compose.yml* also contains port mapping, so the port 80 on the guest system is mapped to the port 5000 on the host system.
You can use any other port in case of any possible collision.

f) To access the app you need to know IP address of the container.
To get the IP with Docker Toolbox use the following command
 docker-machine ip default

## STEP 3 Enjoy docker-compose
Just run 
```
 docker-compose up
```
to build/start the container
and 
```
 docker-compose down 
```
to stop it. 

## STEP 4 Use teamcitytheatre running in your container
Open http://<IP_OF_YOUR_RUNNING_CONTAINER>:5000 in the web browser.

# Display branches
One more small feature is a "display branch". A branch can contain some fixed text in the name which would be nice to exclude. E.g. instead of *refs/heads/1245* or *f/PHX-5555* or *heads/master* one may want to display just the significant parts, *1245*, *PHX-5555* and *master* correspondingly. To do it just use DisplayBranches in *configuration.json*.  

```
,
  "DisplayBranches": [
    "f/(PHX-.*)",
    "refs/heads/(.*)",
    "(master)",
  ]
}
```
These are the Regex strings with one Regex group. If the original branch name matches any string the extracted group is used in UI instead of the original branch name.
