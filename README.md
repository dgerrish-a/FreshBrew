
How to build a docker image?
============================
 1) On windows, install docker on windows
 2) Open cmd, cd to freshbrewapiload [one level above Dockerfile]
 3) run  
   docker build -t <image-name> -f FreshBrewApi/Dockerfile .


How it works?
============
	* freshbrewapiload\FreshBrewApi has config.json that drives load. Change its port if needed.
	* Dockerfile is under freshbrewapiload\FreshBrewApi. 
		* to override default port 8080 to port 9008, change line 
				ENTRYPOINT ["dotnet", "FreshBrewApi.dll"]
			to
				ENTRYPOINT ["dotnet", "FreshBrewApi.dll","/port:value=9008"]
	and rebuild docker image

How to push to Zeus?
===================
	
You need to tag your image correctly first with your registryhost:
				docker tag [OPTIONS] IMAGE[:TAG] [REGISTRYHOST/][USERNAME/]NAME[:TAG]
		Then docker push using that same tag.
				docker push NAME[:TAG]
		Example:

			docker tag 518a41981a6a myRegistry.com/myImage
			docker push myRegistry.com/myImage

		So, for Zeus tag your image correctly first:
			docker tag f7cb0d7f3e11 zeus.run/automation/java_ims_load:1.0
