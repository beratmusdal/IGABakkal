getAllItem();

document.addEventListener('DOMContentLoaded', function () {
    getAllItem();

    const barcodeInput = document.getElementById('barcodeInput');

    if (barcodeInput) {
        barcodeInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                const barcode = barcodeInput.value.trim();
                if (barcode) {
                    addToCart(barcode);
                    barcodeInput.value = '';
                }
            }
        });
        barcodeInput.addEventListener('input', function () {
            const searchValue = barcodeInput.value.trim().toLowerCase();
            const productCards = document.querySelectorAll('.product-card');

            productCards.forEach(card => {
                const name = card.getAttribute('data-name')?.toLowerCase() || '';
                const barcode = card.getAttribute('data-id')?.toLowerCase() || '';

                if (name.includes(searchValue) || barcode.includes(searchValue)) {
                    card.style.display = '';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    }
});


function clearCart() {
    fetch('/Satis/removeAllItem', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            saleDesign(data);
        })
        .catch(error => {
            console.error("Hata:", error);
        });
}
function addToCart(barcode) {
    const productElement = document.querySelector(`.product-card[data-id="${barcode}"]`);

    if (!productElement) {
        console.error("Ürün bulunamadı!");
        return;
    }

    addProductToCart(barcode)
}
function addProductToCart(barcode) {
    fetch('/Satis/increaseItem', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(barcode)
    })
        .then(async response => {
            if (!response.ok) {
                const errorMessage = await response.text();
                alert(errorMessage); 
                return;
            }

            return response.json();
        })
        .then(data => {
            if (data) {
                saleDesign(data);
            }
        })
        .catch(error => {
            console.error("Hata:", error);
            alert("Bir hata oluştu.");


        });
}
function removeFromCart(barcode) {
    fetch('/Satis/decreaseItem', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(barcode)
    })
        .then(response => response.json())
        .then(data => {

            saleDesign(data)
        })
        .catch(error => {
            console.error("Hata:", error);
        });
}
function offerFree(barcode) {
    fetch('/Satis/Gift', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(barcode)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {                
                getAllItem();
            } else {
                alert("Bir hata oluştu: " + data.message);
            }
        })
        .catch(error => {
            console.error('Hata:', error);
            alert('Bir hata oluştu.');
        });
}
function removeItemCompletely(barcode) {
    fetch('/Satis/deleteItem', {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(barcode)
    })
        .then(response => response.json())
        .then(data => {

            saleDesign(data)
        })
        .catch(error => {
            console.error("Hata:", error);
        });
}
function getAllItem() {
    fetch('/Satis/getAllItem', {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            saleDesign(data)
        })
        .catch(error => {
            console.error("Hata:", error);
        });
}
function saleDesign(data) {
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');
    cartItems.innerHTML = '';
    let total = 0;

    Object.values(data).forEach(item => {
        const itemTotal = item.price * item.quantity;
        total += itemTotal;

        const cartItem = document.createElement('div');
        cartItem.className = 'cart-item';
        cartItem.innerHTML = `
            <div class="cart-item-info">
                <h5>${item.name}</h5>
                <p>${item.price} TL x ${item.quantity}</p>
                <p>Toplam: <span class="item-total">${itemTotal} TL</span></p>
            </div>
            <div class="cart-item-quantity">
                <button onclick="removeFromCart('${item.barcode}')">-</button>
                <span>${item.quantity}</span>
                <button onclick="addProductToCart('${item.barcode}')">+</button>
            </div>
            <button class="remove-item" onclick="removeItemCompletely('${item.barcode}')">
                <i class="fas fa-times"></i>
            </button>

            <!-- İkram Et Butonu -->
            <button class="btn btn-warning mt-2" onclick="offerFree('${item.barcode}')">
              <i class="fas fa-gift"></i>
            </button>
        `;

        cartItems.appendChild(cartItem);
    });

    cartTotal.textContent = `${total} TL`;
}
function completepay() {
    let cartItems = [];
    let totalAmount = parseFloat(document.getElementById("cartTotal")?.innerText.replace("TL", "").trim()) || 0;


    fetch('/Satis/getAllItem', {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log(data)
            Object.values(data).forEach(item => {
                cartItems.push({
                    barcode: item.barcode,
                    quantity: item.quantity,
                    unitPrice: item.price
                });
            });
            if (cartItems.length === 0) {
                alert("Sepetiniz boş!");
                return;
            }

            let saleData = {
                totalAmount: totalAmount,
                saleItems: cartItems
            };

            fetch("/Satis/CompleteSale", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(saleData)
            })
                .then(response => {
                    if (!response.ok) throw new Error("Sunucu hatası!");
                    return response.json();
                })
                .then(data => {
                    alert("Satış tamamlandı!");
                    location.reload();
                })
                .catch(error => {
                    console.error("Hata:", error);
                    alert("Satış tamamlanırken bir hata oluştu!");
                });
        })
        .catch(error => {
            console.error("Hata:", error);
        });   

}