// winget list | findstr k6                     
// winget install k6 --source winget       
// & 'C:\Program Files\k6\k6.exe' run test.js
import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
        { duration: '30s', target: 50 }, // Ramp up to 50 users over 30 seconds
        { duration: '1m', target: 50 },  // Stay at 50 users for 1 minute
        { duration: '30s', target: 0 },  // Ramp down to 0 users over 30 seconds
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests should complete below 500ms
        http_req_failed: ['rate<0.01'],   // Error rate should be less than 1%
    },
};

export default function () {
    const urls = [
        // 'http://localhost:5188/weatherforecast',
        'http://localhost:5240/api/Book?pageIndex=0&pageSize=10',
        'http://localhost:5240/api/Book?pageIndex=0&pageSize=8',
        'http://localhost:5240/api/Book?pageIndex=0&pageSize=9',
        'http://localhost:5240/api/Book?pageIndex=0&pageSize=7',
        'http://localhost:5240/api/Book?pageIndex=0&pageSize=5',
    ];

    urls.forEach((url) => {
        const res = http.get(url);
        check(res, {
            'is status 200': (r) => r.status === 200,
        });
    });

    // Simulate user think time
    sleep(1);
}
