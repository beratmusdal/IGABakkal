getAllItem();



function handleEnterKey() {
    const barcodeInput = document.getElementById('barcodeInput');
    const barcode = barcodeInput.value;

    if (barcodeInput.value.length == 13) {
        addToCart(barcode);
        barcodeInput.value = '';
    }
}

function clearCart() {
    if (confirm('Sepetteki tüm ürünleri silmek istediğinizden emin misiniz?')) {
        fetch('/Satis/removeAllItem', {
            method: "POST",
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
        .then(response => response.json())
        .then(data => {

            saleDesign(data)
        })
        .catch(error => {
            console.error("Hata:", error);
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
                <p>Toplam: ${itemTotal} TL</p>
            </div>
            <div class="cart-item-quantity">
                <button onclick="removeFromCart('${item.barcode}')">-</button>
                <span>${item.quantity}</span>
                <button onclick="addProductToCart('${item.barcode}')">+</button>
            </div>
            <button class="remove-item" onclick="removeItemCompletely('${item.barcode}')">
                <i class="fas fa-times"></i>
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