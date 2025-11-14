function printAttendance() {
    const grid = document.querySelector("#print-area table");
    if (!grid) {
        alert("MudDataGrid table not found!");
        return;
    }

    let html = '<table border="1" cellpadding="6" cellspacing="0" style="width:100%; border-collapse: collapse;">';


    html += '<thead><tr>';
    grid.querySelectorAll('thead th').forEach(th => {
        html += `<th>${th.innerText}</th>`;
    });
    html += '</tr></thead>';


    html += '<tbody>';
    grid.querySelectorAll('tbody tr').forEach(tr => {
        html += '<tr>';
        tr.querySelectorAll('td').forEach(td => {
            html += `<td>${td.innerText.trim()}</td>`;
        });
        html += '</tr>';
    });
    html += '</tbody></table>';


    const printWindow = window.open("_blank");
    printWindow.document.write(`
        <html>
        <head><title>Attendance Report</title></head>
        <body>
            ${html}
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.print();
}
