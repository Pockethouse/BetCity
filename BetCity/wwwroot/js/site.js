// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const body = document.body;
    if (body.scrollHeight > window.innerHeight) {
        body.classList.remove("no-scroll");
    } else {
        body.classList.add("no-scroll");
    }
});
