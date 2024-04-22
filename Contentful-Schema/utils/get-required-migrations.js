import { readdir } from 'node:fs/promises';
import minimist from 'minimist';
import chalk from 'chalk';
import core from '@actions/core';

const argv = minimist(process.argv.slice(2));
const currentVersion = parseInt(argv.currentVersion);

const files = await readdir('../migrations');

var requiredMigrations = [];
for (var x=0; x<files.length; x++) {
    console.log (files[x]);
    var version = parseInt(files[x].split('-')[0]);
    if (version > currentVersion) {
        requiredMigrations = files.slice(version - 1);
        break;
    }
}
console.log(requiredMigrations);
core.setOutput('required-migrations', requiredMigrations);
