const select = document.querySelector('select');
const allLang = ['en', 'ru'];

var currentLamguage;  

const langArr = {
    "WelcomeText": {
        "en": "Welcome to ICollections!",
        "ru": "Добро пожаловать на ICollections!",
    },
    "LogIn": {
        "en": "Log in!",
        "ru": "Войдите!",
    },
    "notacc": {
        "en": "You don't have an account?",
        "ru": "У вас нет аккаунта?",
    },
    "EnterEmailLogin": {
        "en": "Email:",
        "ru": "Электронная почта:",
    },
    "EnterPasswordLogin": {
        "en": "Password:",  
        "ru": "Пароль:",
    },
    "InputLogin": {
        "en": "Login!",
        "ru": "Войти!",
    },
    "TextInUnregisteredIndex": {
        "en": "Register or Login to post your own collections or write comments.",
        "ru": "Зарегистрируйтесь или войдите для того, чтобы публиковать свои коллекции и писать комментарии.",
    },
    "RememberMe": {
        "en": "Remember Me?",
        "ru": "Запомнить меня?",
    },
    "HomeLayout": {
        "en": "Home",
        "ru": "Домой",
    },
    "RegisterLayout": {
        "en": "Register",
        "ru": "Зарегистрироваться",
    },
    "LoginLayout": {
        "en": "Login",
        "ru": "Войти",
    },
    "ProfileDropDown": {
        "en": "Profile",
        "ru": "Профиль",
    },
    "LogoutDropDown": {
        "en": "Logout",
        "ru": "Выйти",
    },
    "FirstNameRegister": {
        "en": "First Name:",
        "ru": "Имя:",
    },
    "LastNameRegister": {
        "en": "Last Name:",
        "ru": "Фамилия:",
    },
    "NickNameRegister": {
        "en": "Nickname:",
        "ru": "Ник:",
    },
    "AgeRegister": {
        "en": "Age:",
        "ru": "Возраст:",
    },
    "ConfirmPasswordRegister": {
        "en": "Confirm password:",
        "ru": "Подтвердите пароль:",
    },
    "RegisterButton": {
        "en": "Register!",
        "ru": "Зарегистрироваться!",
    },
    "NewUserRegistration": {
        "en": "New User Registration:",
        "ru": "Регистрация нового пользователя:",
    },
    "ProfileRegistrationDate": {
        "en": "Registration date:",
        "ru": "Дата регистрации:",
    },
    "ProfileRole": {
        "en": "Role:",
        "ru": "Роль:",
    },
    "ProfileText": {
        "en": "Your profile:",
        "ru": "Ваш профиль:",
    },
    "AdminPanel": {
        "en": "Admin panel",
        "ru": "Панель администратора",
    },
    "UserStatus": {
        "en": "User status:",
        "ru": "Статус пользователя:",
    },
    "Delete": {
        "en": "Delete",
        "ru": "Удалить",
    },
    "Block": {
        "en": "Block",
        "ru": "Заблокировать",
    },
    "Unblock": {
        "en": "Unblock",
        "ru": "Разблокировать",
    },
    "Promote": {
        "en": "Promote to admin",
        "ru": "Повысить до администратора",
    },
    "Demote": {
        "en": "Demote to user",
        "ru": "Понизить до пользователя",
    },
    //TODO
    //Fix repetitions
    "Logout": {
        "en": "Logout",
        "ru": "Выйти",
    },
}

select.addEventListener('change',changeURLLanguage);

function changeURLLanguage() {
    let lang = select.value;
    localStorage.setItem('lang', lang);
    location.href = window.location.pathname + "#" + lang;
    location.reload();
}

function changeLanguage() {
    let lang = localStorage.getItem('lang');
    if (!allLang.includes(lang)) {
        location.href =  window.location.pathname + '#en';
        lang = "en";
        localStorage.setItem('lang', lang);
        console.log(location.href);
    }
    location.href = window.location.pathname + "#" + lang;
    select.value = lang;
    for (let key in langArr) {
        let elem = document.querySelector(".lng-" + key);
        if (elem) {
            elem.innerHTML = langArr[key][lang];
        }
    }
}

function getCurrentLanguage() {
    return window.location.hash.substring(1);
}

changeLanguage();