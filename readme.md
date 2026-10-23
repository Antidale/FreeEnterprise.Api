# Free Enterprise API
|branch|status|
|---|---|
|Main| [![Build Status](https://dev.azure.com/antidale/antidale/_apis/build/status/Antidale.FreeEnterprise.Api?branchName=main)](https://dev.azure.com/antidale/antidale/_build/latest?definitionId=1&branchName=main)|

This is used to support various small aspects of providing information about the [FF4: Free Enterprise](https://ff4fe.com/) randomizer. The data is used to power https://info.tellah.life/ and https://tellah.life/, as well as the [Tellah's Library](https://github.com/Antidale/tellahs-library#a-discord-bot-for-free-enterprise) discord bot.

This is currently hosted on Heroku at https://free-enterprise-info-api.herokuapp.com/, and has an accesible [Open API](https://free-enterprise-info-api.herokuapp.com/scalar) defintion.

See [free_enterprise_db](https://github.com/Antidale/free_enterprise_db) for how to set up the database.  Also, a mostly complete version of the database is created when using the compose.yaml to run this project and the database in a container. Tested as working with both Podman and Docker. Some tables are net set up or populated, however, so the containerization is not yet a full replacement for maintaining your own standalone database.


