let redirectUrlLink = document.getElementById('redirect-url');
if (redirectUrlLink != null) {
    redirectUrlLink.addEventListener('click', () => {
        let redirectUrl = window.location.pathname + window.location.search;
        localStorage.setItem("redirectUrl", redirectUrl);
    });
}