# Migrations

The migrations directory should contain:
* a .cjs script file for every migration, with the file name following the process below
* a manifest.txt file that details each of the migration script files
* a tarball called migrations.tar.gz which contains all the migration script files and the manifest

## Migration Script Naming

The format for naming script files is: [number]-description-of-scripts-action.cjs
When creating a new migration script the number should be one higher than the highest number existing script file.
If you're creating a migration which creates a new content type called 'foo' and the current highest numbered script is 0009 you should create a file called 0010-create-foo-content-type.cjs

## Manifest File

When creating a new migration script file, you must add the name of your script file to the manifest.txt file on a new line.  Don't include any whitespace, and don't leave a newline at the end of the file.

## Updating the tarball

When creating a new migration script file, as well as checking it into the Git repo, you must add it to the migrations.tar.gz archive.  You do that by deleting and recreating the archive file.  After completing your script file and updating the manifest.txt file, run the following command in the migrations directory:

`rm migrations.tar.gz && tar -zcf migrations.tar.gz *.cjs manifest.txt`

And then check the archive file into the Git repo. 