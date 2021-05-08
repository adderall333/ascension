let redirectUrlLink = document.getElementById('redirect-url');
if (redirectUrlLink != null) {
    redirectUrlLink.addEventListener('click', () => {
        let redirectUrl = window.location.pathname;
        localStorage.setItem("redirectUrl", redirectUrl);
    });
}