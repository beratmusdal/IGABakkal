
    $(document).ready(function () {
        $('#monthlyClosuresTable tbody').empty();
    $('#monthlyTotalSales').text("0");
    $('#monthlyTotalRefund').text("0");
    $('#closingCount').text("0");
    $('#totalFireQuantity').text("0");
    $('#totalGiftQuantity').text("0");
    });

    $('#listClosings').on('click', function () {
        var month = $('#closingMonth').val();
    if (!month) {
        alert("Lütfen geçerli bir ay seçin.");
    return;
        }

    $.ajax({
        url: '/Monthly/GetMonthlyClosures',
    method: 'GET',
    data: {month: month },
    success: function (data) {
        $('#monthlyClosuresTable tbody').empty();

    let totalSales = 0;
    let totalRefund = 0;
    let totalFireQuantity = 0;
    let totalGift = 0;
    let closingCount = data.length;

    data.forEach(function (item) {
                    const date = new Date(item.createDate);
    const formattedDate = ("0" + date.getDate()).slice(-2) + "-" + ("0" + (date.getMonth() + 1)).slice(-2) + "-" + date.getFullYear();
    const isoDate = date.toISOString().split('T')[0]; // YYYY-MM-DD

    $('#monthlyClosuresTable tbody').append(`
    <tr>
        <td>${formattedDate}</td>
        <td>${item.zNo ?? "-"}</td>
        <td>${item.totalAmount} ₺</td>
        <td>${item.totalRefund} ₺</td>
        <td>${item.totalFireQuantity}</td>
        <td>${item.totalGiftQuantity}</td>
        <td>${item.personelName}</td>
        <td>
            <button class="btn btn-sm btn-info btn-detail" data-date="${isoDate}">
                <i class="fas fa-eye"></i> Detay
            </button>
        </td>
    </tr>
    `);

    totalSales += item.totalAmount;
    totalRefund += item.totalRefund;
    totalFireQuantity += item.totalFireQuantity;
    totalGift += item.totalGiftQuantity;
                });

    $('#monthlyTotalSales').text(totalSales + " ₺");
    $('#monthlyTotalRefund').text(totalRefund + " ₺");
    $('#closingCount').text(closingCount);
    $('#totalFireQuantity').text(totalFireQuantity);
    $('#totalGiftQuantity').text(totalGift);
            },
    error: function () {
        alert("Listeleme sırasında bir hata oluştu.");
            }
        });
    });

    $('#generateMonthlyReport').on('click', function () {
        let reportData = [];

    $('#monthlyClosuresTable tbody tr').each(function () {
            const tds = $(this).find('td');
    if (tds.length < 7) return;

    const parts = tds.eq(0).text().trim().split('-');
    const isoDate = `${parts[2]}-${parts[1]}-${parts[0]}T00:00:00`;

    reportData.push({
        CreateDate: isoDate,
    ZNo: tds.eq(1).text(),
    TotalAmount: Number(tds.eq(2).text().replace('₺', '').trim()),
    TotalRefund: Number(tds.eq(3).text().replace('₺', '').trim()),
    TotalFireQuantity: parseInt(tds.eq(4).text()),
    TotalGiftQuantity: parseInt(tds.eq(5).text()),
    PersonelName: tds.eq(6).text()
            });
        });

    $.ajax({
        url: '/Monthly/GenerateMonthlyPdf',
    method: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(reportData),
    success: function () {
        window.open('/reports/monthly_filled.pdf', '_blank');
            },
    error: function () {
        alert("PDF oluşturulamadı.");
            }
        });
    });

    // Detay butonuna tıklandığında satışları getiren popup
    $(document).on('click', '.btn-detail', function () {
        const dateStr = $(this).data('date');

    $.ajax({
        url: '/Monthly/GetDetailOfMonthlyClosing',
    method: 'GET',
    data: {date: dateStr },
    success: function (data) {
                if (!data || data.length === 0) {
        Swal.fire({
            title: 'Detay',
            html: '<p>Bu güne ait satış kaydı bulunamadı.</p>',
            icon: 'info'
        });
    return;
                }

    let listHtml = `
    <table class="table table-bordered mb-0">
        <thead>
            <tr>
                <th>ID</th>
                <th>Saat</th>
                <th>Tutar</th>
                <th>Fiş</th>
            </tr>
        </thead>
        <tbody>
            `;

                data.forEach(x => {
                listHtml += `
                        <tr class="main-sale-row">
                            <td>${x.id}</td>
                            <td>${x.createDate}</td>
                            <td>${x.amount} ₺</td>
                            <td>
                                <button class="btn btn-sm btn-secondary btn-toggle-details" data-saleid="${x.id}">➕</button>
                            </td>
                        </tr>
                        <tr class="detail-row" id="details-${x.id}" style="display:none;">
                            <td colspan="4" class="bg-light text-center"><em>Yükleniyor...</em></td>
                        </tr>
                    `;
                });

            listHtml += `</tbody></table>`;

    Swal.fire({
        title: 'Günlük Satışlar',
    html: listHtml,
    width: 800,
    showConfirmButton: false
                });
            },
    error: function () {
        Swal.fire("Hata", "Detaylar yüklenirken bir hata oluştu.", "error");
            }
        });
    });

    // Satış içindeki ürün detaylarını aç/kapat
    $(document).on('click', '.btn-toggle-details', function () {
        const saleId = $(this).data('saleid');
    const detailRow = $(`#details-${saleId}`);

    if (detailRow.is(':visible')) {
        detailRow.hide();
    $(this).html('➕');
    return;
        }

    $.ajax({
        url: '/Monthly/GetSaleItems',
    method: 'GET',
    data: {saleId: saleId },
    success: function (data) {
        let html = `
    <table class="table table-sm table-striped mb-0">
        <thead>
            <tr>
                <th>Ürün</th>
                <th>Adet</th>
                <th>Birim Fiyat</th>
                <th>Ara Toplam</th>
            </tr>
        </thead>
        <tbody>
            `;

            let total = 0;

                data.forEach(x => {
                    const araToplam = x.unitPrice * x.quantity;
            total += araToplam;

            html += `
            <tr>
                <td>${x.productName}</td>
                <td>${x.quantity}</td>
                <td>${x.unitPrice.toLocaleString('tr-TR')} ₺</td>
                <td>${araToplam.toLocaleString('tr-TR')} ₺</td>
            </tr>
            `;
                });

            html += `
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3"><strong>Toplam</strong></td>
                <td><strong>${total.toLocaleString('tr-TR')} ₺</strong></td>
            </tr>
        </tfoot>
    </table>
    `;

    detailRow.html(`<td colspan="4">${html}</td>`).fadeIn();
    $(`.btn-toggle-details[data-saleid="${saleId}"]`).html('➖');
            },
    error: function () {
        detailRow.html('<td colspan="4" class="text-danger">Detaylar alınamadı.</td>').fadeIn();
            }
        });
    });

    $('#exportExcelReport').on('click', function () {
        var month = $('#closingMonth').val();
    if (!month) {
        alert("Lütfen geçerli bir ay seçin.");
    return;
        }

    window.location.href = '/Monthly/ExportMonthlyProductSalesToExcel?month=' + month;
    });
