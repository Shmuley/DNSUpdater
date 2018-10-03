# Contributing
 The goal of the project would to be to have a service that will work with many different registrars, so the biggest contribution you can make is by adding more registrars.

## Adding a Registrar
Some of the things I want to do is generalize the code as much as possible. I'm tyring to make the call in the service as generic as possibel, so this should allow you to implement the interfaces for domains and records in your own classes, then call the ApiCaller passing in the new classes.