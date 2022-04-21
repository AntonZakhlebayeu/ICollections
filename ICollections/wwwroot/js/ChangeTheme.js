let changeThemeButtons = document.querySelectorAll('.changeTheme');

changeThemeButtons.forEach(button => {
    button.addEventListener('click', function () {
        let theme = this.dataset.theme;
        applyTheme(theme);
    });
});

function applyTheme(themeName) {
    console.log(themeName);
    let theme = `/css/Shared/theme-${themeName}.css`;
    document.querySelector('[title="theme"]').setAttribute('href', theme);
    changeThemeButtons.forEach(button => {
        button.style.display = 'block';
    });
    document.querySelector(`[data-theme="${themeName}"]`).style.display = 'none';
    localStorage.setItem('theme', themeName);
}

let activeTheme = localStorage.getItem('theme'); 

if (activeTheme === null || activeTheme === 'light') { 
    applyTheme('light');
} else if (activeTheme === 'dark') { 
    applyTheme('dark');
}