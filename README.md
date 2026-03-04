# Callbi Dungeon Explorer

Clone the repo.

From the solution root run 
	"docker-compose up --build"

The frontend is deployed to "http://localhost:3000" and the API to "http://localhost:8080/api/"

On first run register any email address and password and then login.

If you want to change the client app port in the container, you'll need to change the API config setting "FrontendUrl" in appsetting.json as well.

From Postman you'll need to get a token from the auth/login method and set this as the Bearer token for all calls to api/dungeons

There is Swagger documentation at "http://localhost:8080/swagger". You'll also need to call the auth/login endpoint to get a token.



