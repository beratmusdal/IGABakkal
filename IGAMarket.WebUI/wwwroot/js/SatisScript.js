//let cart = {};
//let tempBarcode = null; // Geçici barkod değişkeni


//// Ödeme işlemi
//function completepay() {
//    let cartItems = [];
//    let totalAmount = parseFloat(document.getElementById("cartTotal")?.innerText.replace("TL", "").trim()) || 0;

//    // Sepetteki tüm ürünleri cart nesnesinden al
//    Object.values(cart).forEach(item => {
//        cartItems.push({
//            productId: parseInt(item.barcode), // ID'yi tam sayı olarak gönderiyoruz
//            quantity: item.quantity,
//            unitPrice: item.salePrice
//        });
//    });

//    if (cartItems.length === 0) {
//        alert("Sepetiniz boş!");
//        return;
//    }

//    let saleData = {
//        totalAmount: totalAmount,
//        saleItems: cartItems
//    };

//    fetch("/Satis/CompleteSale", {
//        method: "POST",
//        headers: {
//            "Content-Type": "application/json"
//        },
//        body: JSON.stringify(saleData)
//    })
//        .then(response => {
//            if (!response.ok) throw new Error("Sunucu hatası!");
//            return response.json();
//        })
//        .then(data => {
//            alert("Satış tamamlandı!");
//            location.reload();
//        })
//        .catch(error => {
//            console.error("Hata:", error);
//            alert("Satış tamamlanırken bir hata oluştu!");
//        });
//}


//function handleEnterKey() {
//    const barcodeInput = document.getElementById('barcodeInput');


//    const barcode = barcodeInput.value;

//    if (barcodeInput.value.length == 13) {
//        addToCart(barcode);
//        barcodeInput.value = '';
//    }
//}


//function addToCart(barcode) {
//    // Ürünün HTML'deki ilgili elemanını bul
//    const productElement = document.querySelector(`.product-card[data-id="${barcode}"]`);

//    if (!productElement) {
//        console.error("Ürün bulunamadı!");
//        return;
//    }

//    // Ürün bilgilerini data-* attribute'larından al
//    const name = productElement.getAttribute('data-name');
//    const salePrice = parseFloat(productElement.getAttribute('data-price'));
//    const image = productElement.getAttribute('data-image');

//    if (cart[barcode]) {
//        cart[barcode].quantity++;
//    } else {
//        cart[barcode] = { barcode, name, salePrice, image, quantity: 1 };
//    }

//    updateCartDisplay();
//}


//// Update the cart display
//// Sepet güncellenince localStorage'a kaydedin
//function updateCartDisplay() {
//    const cartItems = document.getElementById('cartItems');
//    const cartTotal = document.getElementById('cartTotal');
//    cartItems.innerHTML = ''; // Clear the cart
//    let total = 0;

//    Object.values(cart).forEach(item => {
//        const itemTotal = item.salePrice * item.quantity; // Total price for this item
//        total += itemTotal;

//        const cartItem = document.createElement('div');
//        cartItem.className = 'cart-item';
//        cartItem.innerHTML = `
//            <div class="cart-item-info">
//                <h5>${item.name}</h5>
//                <p>${item.salePrice} TL x ${item.quantity}</p>
//                <p>Toplam: ${itemTotal} TL</p>
//            </div>
//            <div class="cart-item-quantity">
//                <button onclick="removeFromCart('${item.barcode}')">-</button>
//                <span>${item.quantity}</span>
//                <button onclick="addProductToCart('${item.barcode}')">+</button>
//            </div>
//            <button class="remove-item" onclick="removeItemCompletely('${item.barcode}')">
//                <i class="fas fa-times"></i>
//            </button>
//        `;
//        cartItems.appendChild(cartItem);
//    });

//    cartTotal.textContent = `${total} TL`; // Update total cart price

//    // Sepet verisini localStorage'a kaydedin
//    localStorage.setItem('cart', JSON.stringify(cart));
//}



//// Sepete Ekle fonksiyonunu güncelleme
//function addProductToCart(barcode) {
//    const barcodeInput = document.getElementById('barcodeInput');
//    if (tempBarcode) {
//        fetchProduct(tempBarcode);
//        tempBarcode = null; // İşlem tamamlandıktan sonra sıfırla
//        barcodeInput.value = ''; // Input'u temizle
//    } else {
//        // Eğer barkod okutulmadıysa, direkt olarak ürün kartındaki barkodu kullan
//        fetchProduct(barcode);
//    }
//}

//// Barkoda göre ürünü getir ve sepete ekle
//async function fetchProduct(barcode) {
//    try {
//        const response = await fetch(`/Satis/GetProductByBarcode?barcode=${barcode}`);
//        if (!response.ok) {
//            throw new Error('Ürün bulunamadı!');
//        }

//        const product = await response.json(); // Gelen JSON verisini al
//        addToCart(product.barcode, product.name, product.salePrice, product.imageUrl);
//    } catch (error) {
//        alert(error.message);
//    }
//}



//// Sepetten ürün çıkarma
//function removeFromCart(barcode) {
//    if (cart[barcode].quantity > 1) {
//        cart[barcode].quantity--; // Miktar bir azalacak
//    } else {
//        delete cart[barcode]; // Ürün tamamen silinecek
//    }
//    updateCartDisplay();
//}

//// Ürünü tamamen sepetten kaldırma
//function removeItemCompletely(barcode) {
//    delete cart[barcode];
//    updateCartDisplay();
//}
let cart = JSON.parse(localStorage.getItem('cart')) || {}; // localStorage'dan sepeti yükle veya boş bir nesne oluştur
let tempBarcode = null; // Geçici barkod değişkeni

// Sepeti localStorage'a kaydetme fonksiyonu
function saveCartToLocalStorage() {
    localStorage.setItem('cart', JSON.stringify(cart));
}

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
        cart = [];
        localStorage.removeItem('currentCart');
        updateCartDisplay();
    }
}

function addToCart(barcode) {
    const productElement = document.querySelector(`.product-card[data-id="${barcode}"]`);

    if (!productElement) {
        console.error("Ürün bulunamadı!");
        return;
    }

    const name = productElement.getAttribute('data-name');
    const salePrice = parseFloat(productElement.getAttribute('data-price'));
    const image = productElement.getAttribute('data-image');

    if (cart[barcode]) {
        cart[barcode].quantity++;
    } else {
        cart[barcode] = { barcode, name, salePrice, image, quantity: 1 };
    }

    updateCartDisplay();
    saveCartToLocalStorage();
}

function updateCartDisplay() {
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');
    cartItems.innerHTML = '';
    let total = 0;

    Object.values(cart).forEach(item => {
        const itemTotal = item.salePrice * item.quantity;
        total += itemTotal;

        const cartItem = document.createElement('div');
        cartItem.className = 'cart-item';
        cartItem.innerHTML = `
            <div class="cart-item-info">
                <h5>${item.name}</h5>
                <p>${item.salePrice} TL x ${item.quantity}</p>
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
    saveCartToLocalStorage();
}

function addProductToCart(barcode) {
    const barcodeInput = document.getElementById('barcodeInput');
    if (tempBarcode) {
        fetchProduct(tempBarcode);
        tempBarcode = null;
        barcodeInput.value = '';
    } else {
        fetchProduct(barcode);
    }
}

async function fetchProduct(barcode) {
    try {
        const response = await fetch(`/Satis/GetProductByBarcode?barcode=${barcode}`);
        if (!response.ok) {
            throw new Error('Ürün bulunamadı!');
        }

        const product = await response.json();
        addToCart(product.barcode, product.name, product.salePrice, product.imageUrl);
    } catch (error) {
        alert(error.message);
    }
}

function removeFromCart(barcode) {
    if (cart[barcode].quantity > 1) {
        cart[barcode].quantity--;
    } else {
        delete cart[barcode];
    }
    updateCartDisplay();
    saveCartToLocalStorage();
}

function removeItemCompletely(barcode) {
    delete cart[barcode];
    updateCartDisplay();
    saveCartToLocalStorage();
}

function completepay() {
    let cartItems = [];
    let totalAmount = parseFloat(document.getElementById("cartTotal")?.innerText.replace("TL", "").trim()) || 0;

    Object.values(cart).forEach(item => {
        cartItems.push({
            barcode: item.barcode,
            quantity: item.quantity,
            unitPrice: item.salePrice
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

    cart = {};
    saveCartToLocalStorage();
}
// Sayfa yüklendiğinde sepeti ve görüntüsünü güncelle
window.onload = function () {
    updateCartDisplay();
};