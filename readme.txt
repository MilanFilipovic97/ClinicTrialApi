In order to run this application, you can just run docker compose. 
It will run added migrations which will create the database inside the container.

There are no data seeded so first thing in testing this app is to upload a file.
When file is successfully uploaded a generated Id is returned which can be used for other endpoints.

Inside "test-files" folder in the root of the project I added two json files which were used for testing.
One is valid and the other isn't. 

By running docker compose swagger page will be opened and you will see all added endpoints.

