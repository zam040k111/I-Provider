function changeTegClass(el, className) { el.setAttribute("class", className); }

function HideDeletedPlEvent(el) { window.location.href = "/Admin/HideDeletedPlEvent?value=" + el.checked; }

function HideDeletedTpEvent(el) { window.location.href = "/Admin/HideDeletedTpEvent?value=" + el.checked; }

var obGlobal;

function detailed() {
    let ob = event.currentTarget;
    obGlobal = ob.getBoundingClientRect().top;
    let scale = document.getElementById("bod").clientWidth / ob.clientWidth;
    let prdArr = document.getElementsByClassName("product");
    if (scale > 2)
        $('html, body').animate({ scrollTop: 0 });
    let pointsBod = document.getElementById("bod").getBoundingClientRect();
    let pointsOb = ob.getBoundingClientRect();

    let gapX = Math.trunc((pointsOb.left - pointsBod.left) / ob.clientWidth) * 7.5;
    let gapY = Math.trunc((pointsOb.top - pointsBod.top) / ob.clientHeight) * 7.5;

    let x = (pointsOb.left - pointsBod.left) / Math.trunc(scale - 1);
    let y = (pointsOb.top - pointsBod.top) / Math.trunc(scale - 1);
    x -= gapX;
    y -= gapY;
    changeTegClass(ob.firstElementChild, "prd-first-child is-visible");
    changeTegClass(ob.lastElementChild, "prd-desc");
    ob.style.transformOrigin = x + "px " + y + "px";

    for (var i = 0; i < prdArr.length; i++) {
        prdArr[i].onclick -= detailed;
    }
    changeTegClass(ob, "product loaded detailed");
    if (scale > 2)
        ob.style.transform = "scale(" + scale + ")";
}
function detClose() {
    var ob = event.currentTarget;
    let scale = document.getElementById("bod").clientWidth / ob.clientWidth;
    changeTegClass(ob.parentElement.parentElement, "product loaded");
    ob.parentElement.parentElement.style.transform = "scale(1)";
    changeTegClass(ob.parentElement, "prd-first-child");
    changeTegClass(ob.parentElement.parentElement.lastElementChild, "prd-desc is-visible");
    if (scale > 2 && obGlobal > 100)
        $('html, body').animate({ scrollTop: obGlobal });
    event.stopPropagation();
    let prdArr = document.getElementsByClassName("product");
    for (var i = 0; i < prdArr.length; i++) {
        prdArr[i].onclick = detailed;
    }
}
function unBlock(id) {
    let conf = confirm('При разблокировке с пользователя будут списаны средства согласно тарифу. Разблокировать пользователя?');
    if (!conf) return conf;
    $.ajax({
        type: "GET",
        url: "/Admin/SetIsBlocked",
        data: {
            id: id,
            value: false
        },
        success: function () {
            alert("Пользователь разблокирован!");
            window.location.href = "/Admin/Customers"
        },
        error: function () {
            alert("У пользователя недостаточно средств.");
        }
    })
}
function addToCart(id) {
    $.ajax({
        type: "GET",
        url: "/Cart/AddToCart",
        data: {
            id: id
        },
        success: function (msg) {
            document.getElementById("nmbItem").setAttribute("class", "number-item-cart");
            $("#nmbItem").text(msg);
        }
    })
}
