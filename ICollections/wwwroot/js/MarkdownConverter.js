function GetConvertedString(md, context) {
    let converter = new showdown.Converter();
    let html = converter.makeHtml(md);

    document.getElementById(context).insertAdjacentHTML("afterend", html);
}    