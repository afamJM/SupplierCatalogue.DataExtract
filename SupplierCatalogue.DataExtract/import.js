const request = require('request');
const supplierJson = require('./Suppliers.json');
const total = supplierJson.length;
let imported = 0;
let success = 0;
let failed = 0;

function importSupplier(s) {
    var dest = process.env.API_DEST || 'http://localhost:5000';
    dest += '/api/v1/suppliers/';
    console.log( "Posting To : " + dest );
    request.post(
        dest,
        { json: s },
        (error, response, body) => {
            imported++;
            if (error || response.statusCode >= 300) {
                console.log(`Supplier ${s.business.company_id} failed.\nResponse was ${response.statusCode}\nError was ${error}\n\n`);
                failed++;
            } else {
                success++;
            }

            if (imported === total) {
                console.log(`${success} out of ${total} suppliers were imported, ${failed} failed`);
            }
        }
    )
}

const interval = setInterval(() => {
    importSupplier(supplierJson.shift());
    if (!supplierJson.length) {
        clearInterval(interval);
    }
}, 10);
