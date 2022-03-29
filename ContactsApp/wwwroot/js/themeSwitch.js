const themeSwitch = document.getElementById('theme-toggle');
themeSwitch.addEventListener('click', () => {
    document.body.classList.toggle('dracula-theme',);
    document.body.classList.toggle('atom-one-dark-theme',);
});