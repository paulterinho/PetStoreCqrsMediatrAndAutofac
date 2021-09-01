 <pre>
    ______    _     _____ _                 
    | ___ \  | |   /  ___| |                
    | |_/ /__| |_  \ `--.| |_ ___  _ __ ___ 
    |  __/ _ \ __|  `--. \ __/ _ \| '__/ _ \
    | | |  __/ |_  /\__/ / || (_) | | |  __/
    \_|  \___|\__| \____/ \__\___/|_|  \___|
                                        
</pre>    

# OpenAPI
This is how we are scaffolding our projects. 

## Import OpenAPI to Postman (to test endpoints)
Postman lets us import OpenAPI specifications to create Postman Projects. 

*To do this*

1) Open up Postman, 
2) and select `File > Import...`.
3) Find the `petstore.yml` file located at `src/petstore.yaml`
4) Import.

## Code Scaffolding with NSwag
This is a scaffolding tool we are using. Download the NswagStudio Application located at 
* https://github.com/RicoSuter/NSwag/releases

### How to scaffold using a Visual Editor

0) Download it from https://github.com/RicoSuter/NSwag/wiki/NSwagStudio
1) Open the `NSwag` config file called `petstore.nswag.json` with the `NSwagStudio` application (or from command line, see `Command Line` section later.).
2) Next, copy and paste the contents of `src/petstore.yml` into the "OpenAPI/Swagger Specification" tab in the bottom left of the `NSwagStudio` application (we can't load the file directly unless it's being served by a webserver.).
3) Click the "Generate Files" button in the bottom left.
