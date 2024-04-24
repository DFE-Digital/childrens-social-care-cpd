import core from '@actions/core';
import { readdir, readFile } from 'node:fs/promises';
import chalk from 'chalk';

const migrationsDir = '../migrations/';
const red = chalk.bold.red;

try {

    const file = await readFile(migrationsDir + 'manifest.txt', { encoding: 'utf8' });
    const manifestFiles = file.split('\n');

    const isMigration = filename => {
        return filename.includes('.cjs');
    }
    const directoryFiles = await readdir(migrationsDir);
    const directoryMigrations = directoryFiles.filter(isMigration);
    var errors = [];

    if (manifestFiles.length !== directoryMigrations.length) {
        errors.push('Manifest and archive contain inconsistent number of migrations');
    }

    manifestFiles.forEach(manifestFilename => {
        if (!directoryMigrations.includes(manifestFilename)) {
            errors.push('Manifest contains migration script ' + manifestFilename + ' which is not present in migrations archive');
        }
    });

    directoryMigrations.forEach(directoryFilename => {
        if (!manifestFiles.includes(directoryFilename)) {
            errors.push('Migrations archive contains migration script ' + directoryFilename + ' which is not present in manifest');
        }
    });

    if (errors.length > 0) {
        console.log(red(errors.join('\n')));
        core.setFailed(red('Mismatch between migrations archive and manifest'));
    }
}
catch (e) {
    console.error(e);
    core.setFailed(red('Unmable to verify migrations against manifest.'));
}

