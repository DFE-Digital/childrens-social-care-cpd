# Contentful Data Model

This folder contains all the Contentful models and their respective migration scripts. Migration in this context can be anything that creates new or modifies existing models or data.

## base-model.js
This is the 'clean install' script that contains the initial complete content model.

Subsequent updates to the model should be maintained in a ```Releases``` folder under an appropriate directory structure in YYYYDDMM format, e.g. ```Releases\20230701```, ```Releases\202308122``` etc.

## Generating migration scripts
The content models of two environments can be compared and the differences exported into a migration script (javascript file) using the [Contentful Merge app](https://www.contentful.com/marketplace/app/merge/), which should be installed in both of the environments you wish to compare. This app can also be driven from the CLI using the Contentful-CLI [merge export](https://github.com/contentful/contentful-cli/tree/master/docs/merge/export) command.

### Important
Currently the Merge app has a bug when exporting the migration to a javascript file, where it will mess up the editors/help text of the models. The fix is manual at the moment, you have to edit the file and check all calls to `changeFieldControl` to ensure the correct parameters are being passed with the correct field name.

## Migrating data
Contentful provides a way to process the data of an environment in a migration script. See [transformEntries](https://github.com/contentful/contentful-migration#transformentriesconfig) in the [Contentful Migration](https://github.com/contentful/contentful-migration) documentation for more information.

