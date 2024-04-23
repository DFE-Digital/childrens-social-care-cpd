import { readdir, access } from 'node:fs/promises';
import minimist from 'minimist';
import chalk from 'chalk';
import core from '@actions/core';

const argv = minimist(process.argv.slice(2));
const currentVersion = parseInt(argv.currentVersion);
const migrationsDir = '../migrations';

try {
    await access(migrationsDir);
}
  catch (error) {
    core.setFailed(chalk.red("Migrations directory doesn't exist"));
    process.exit();
}

const files = await readdir(migrationsDir);

var requiredMigrations = [];
for (var x=0; x<files.length; x++) {
    var version = parseInt(files[x].split('-')[0]);
    if (version > currentVersion) {
        requiredMigrations = files.slice(version - 1);
        break;
    }
}

core.setOutput('required-migrations', requiredMigrations);
