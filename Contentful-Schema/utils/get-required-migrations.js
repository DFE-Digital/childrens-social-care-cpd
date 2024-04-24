import { readdir, access } from 'node:fs/promises';
import minimist from 'minimist';
import chalk from 'chalk';
import core from '@actions/core';

const argv = minimist(process.argv.slice(2));
const currentVersion = parseInt(argv.currentVersion);
const migrationsDir = '../migrations';
const isMigration = filename => {
    return filename.includes('.cjs');
}

try {
    await access(migrationsDir);
}
  catch (error) {
    core.setFailed(chalk.red("Migrations directory doesn't exist"));
    process.exit();
}

var files = await readdir(migrationsDir);
files = files.filter(isMigration);

var requiredMigrations = [];
files.every(filename => {
    var version = parseInt(filename.split('-')[0]);
    if (version > currentVersion) {
        requiredMigrations = files.slice(version - 1);
        return false;
    }
});

core.setOutput('required-migrations', requiredMigrations);
