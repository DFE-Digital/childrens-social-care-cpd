const http = require('http');
const url = require('url');
const fs = require('fs');
const sqlite3 = require('sqlite3').verbose();

// Hardcoded Credentials
const hardcodedPassword = "VeryInsecurePassword123";
const apiKey = "12345-ABCDE-67890-FGHIJ";

const server = http.createServer((req, res) => {
    const parsedUrl = url.parse(req.url, true);
    const pathname = parsedUrl.pathname;
    const query = parsedUrl.query;

    if (pathname === '/api') {
        // SQL Injection Vulnerability
        const userId = query.id;
        const db = new sqlite3.Database('database.db');
        db.get(`SELECT * FROM users WHERE id = ${userId}`, (err, row) => {
            if (err) {
                res.writeHead(500, { 'Content-Type': 'text/plain' });
                res.end('Internal Server Error');
                return;
            }
            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(row));
        });
        db.close();
    } else if (pathname === '/upload') {
        if (req.method === 'POST') {
            let body = '';
            req.on('data', chunk => {
                body += chunk.toString();
            });
            req.on('end', () => {
                // Path Traversal Vulnerability
                const filePath = query.filePath;
                fs.readFile(filePath, (err, data) => {
                    if (err) {
                        res.writeHead(500, { 'Content-Type': 'text/plain' });
                        res.end('Internal Server Error');
                        return;
                    }
                    res.writeHead(200, { 'Content-Type': 'text/plain' });
                    res.end(data);
                });
            });
        }
    } else {
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Not Found');
    }
});

server.listen(3000, () => {
    console.log('Server running at http://localhost:3000/');
});
