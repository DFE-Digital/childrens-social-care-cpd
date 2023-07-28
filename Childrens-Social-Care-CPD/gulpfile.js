/// <binding ProjectOpened='js:watch, sass-to-min-css:watch' />
"use strict";

var tsScriptsSrc = './scripts/**';

var gulp = require("gulp"),
    sourcemaps = require('gulp-sourcemaps'),
    terser = require('gulp-terser'),
    ts = require("gulp-typescript"),
    //typescript = require('typescript'),
    rollup = require('gulp-better-rollup'),
    //concat = require('gulp-concat'),
    del = require('del');


// https://www.meziantou.net/compiling-typescript-using-gulp-in-visual-studio.htm

//todo: clean to delete files in dest? & tmp folder

var tsProject;

gulp.task('transpile-ts', function () {

    if (!tsProject) {
        tsProject = ts.createProject({
            noImplicitAny: false,
            noEmitOnError: true,
            removeComments: false,
            target: "es6",
            allowJs: true,
            checkJs: true
        });
    }

    //console.log(`TypeScript version: ${typescript.version}`);

    var reporter = ts.reporter.fullReporter();

    var tsResult = gulp.src(tsScriptsSrc)
        .pipe(sourcemaps.init())
        .pipe(tsProject(reporter));

    return tsResult.js
        .pipe(sourcemaps.write())
        .pipe(gulp.dest("./tmp/js"));
});

//todo: try rollup-plugin-preserve-modules to preserve the order of js files (if including govuk)
//todo: any benefit of using rollup-plugin-terser?

//gulp.task('naive-bundle-js', () => {
//    return gulp.src(['./wwwroot/lib/govuk/assets/js/govuk-4.4.1.js', './tmp/js/app.js'])
//        .pipe(sourcemaps.init())
//        .pipe(concat('bundle.js'))
//        // inlining the sourcemap into the exported .js file
//        .pipe(sourcemaps.write())
//        .pipe(gulp.dest('./tmp/js'));
//});

gulp.task('bundle-and-minify-js', () => {
    //    return gulp.src('./tmp/js/bundle.js')
    return gulp.src('./tmp/js/app.js')
        .pipe(sourcemaps.init())
        .pipe(rollup({}, 'es'))
        .pipe(terser())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('./wwwroot/js'));
});

gulp.task('clean', () => {
    return del('./tmp/**');
});

//gulp.task('js', gulp.series('clean', 'transpile-ts', 'naive-bundle-js', 'bundle-and-minify-js'));
gulp.task('js', gulp.series('clean', 'transpile-ts', 'bundle-and-minify-js'));

gulp.task('js:watch', function () {
    gulp.watch(tsScriptsSrc, gulp.series('js'));
});
