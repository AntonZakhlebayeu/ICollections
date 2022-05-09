const select = document.querySelector('select');
const allLang = ['en', 'ru'];

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
    "CreateCollection": {
        "en": "Create Collection",
        "ru": "Создать коллекцию",
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
    "CollectionTitle": {
        "en": "Title:",
        "ru": "Заголовок:",
    },
    "CollectionSmallDescription": {
        "en": "Description:",
        "ru": "Описание:",
    },
    "CollectionTheme": {
        "en": "Theme:",
        "ru": "Тема:",
    },
    "DropImageCollection": {
        "en": "Drop file here or click to upload",
        "ru": "Сбросьте сюда фото или нажмите для загрузки",
    },
    "SelectTheme": {
        "en": "Select theme of your collection",
        "ru": "Выберите тему для своей коллекции",
    },
    "AlcoholTheme": {
        "en": "Alcohol",
        "ru": "Алкоголь",
    },
    "BooksTheme": {
        "en": "Books",
        "ru": "Книги",
    },
    "FilmsTheme": {
        "en": "Films",
        "ru": "Фильмы",
    },
    //TODO
    //Fix repetitions
    "CreateCollectionButton": {
        "en": "Create",
        "ru": "Создать",
    },
    "alcoholDate": {
        "en": "Add date of manufacture",
        "ru": "Добавить дату производства",
    },
    "alcoholBrand": {
        "en": "Add alcohol brand",
        "ru": "Добавить марку алкоголя",
    },
    "booksDate": {
        "en": "Add writing date",
        "ru": "Добавить дату написания",
    },
    "booksBrand": {
        "en": "Add books authors",
        "ru": "Добавить авторов книг",
    },
    "filmsDate": {
        "en": "Add release date to rental",
        "ru": "Добавить дату выпуска в прокат",
    },
    "filmsBrand": {
        "en": "Add studio name",
        "ru": "Добавить название студии",
    },
    "CollectionComments": {
        "en": "Add comments",
        "ru": "Добавить комментарии",
    },
    "FindButton": {
        "en": "Find",
        "ru": "Поиск",
    },
    "YourCollections": {
        "en": "Your Collections:",
        "ru": "Ваши коллекции:",
    },
    "CollectionEmpty": {
        "en": "Your collection is empty, expand it by adding new item in collection.",
        "ru": "Ваша коллекция пуста, расширьте ее добавив туда новые предметы.",
    },
    "AddItemButton": {
        "en": "Add Item",
        "ru": "Добавить Предмет",
    },
    "CollectionEmptyNotUser": {
        "en": "This is collection is empty, author will expand id by adding new items.",
        "ru": "Эта коллекция пуста, автор расширит ее добавив новые предметы.",
    },
    "Edit": {
        "en": "Edit",
        "ru": "Редактировать",
    },
    "alcoholItemDate": {
        "en": "Date of manufacture:",
        "ru": "Дата изготовления:",
    },
    "alcoholItemBrand": {
        "en": "Brand:",
        "ru": "Марка:",
    },
    "booksItemDate": {
        "en": "Date of writing:",
        "ru": "Дата написания:",
    },
    "booksItemBrand": {
        "en": "Book author:",
        "ru": "Автор книги:",
    },
    "filmsItemDate": {
        "en": "Release date:",
        "ru": "Дата премьеры:",
    },
    "filmsItemBrand": {
        "en": "Studio:",
        "ru": "Студия:",
    },
    "DeleteImage": {
        "en": "Delete Image",
        "ru": "Удалить картинку",
    },
    "ParentCollection": {
        "en": "Collection:",
        "ru": "Коллекция:",
    },
    "SearchResultFalse": {
        "en": "Nothing found: ",
        "ru": "Ничего не найдено:",
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