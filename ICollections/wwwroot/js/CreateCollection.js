const themeList = {
    "alcohol": {
        "alcohol-date": {
            "class": "lng-alcoholDate",
            "value": "alcoholDate",
        },
        "alcohol-brand": {
            "class": "lng-alcoholBrand",
            "value": "alcoholBrand",
        },
    },
    
    "books": {
        "books-date": {
            "class": "lng-booksDate",
            "value": "booksDate",
        },
        "books-author": {
            "class": "lng-booksAuthor",
            "value": "booksAuthor"
        }
    },
    
    "films": {
        "films-date": {
            "class": "lng-filmsDate",
            "author": "filmsDate",
        },
        "films-author": {
            "class": "lng-filmsAuthor",
            "author": "filmsAuthor",
        }
    }
    
}

var themeSelector = document.getElementById("themeSelector");

themeSelector.addEventListener("change", changeContext)

function changeContext() {
    var selectedTheme = themeSelector.value;
    console.log(selectedTheme);
    
    var additionalVision = document.getElementById("checkBoxAdditional").style;

    if (selectedTheme === "null") {
        additionalVision.display = "none";      
        return;
    }

    additionalVision.display = "block";
    var additionalListInput = document.getElementsByClassName("form-check-input");
    var additionalListLabels = document.getElementsByClassName("form-check-label");

    switch (selectedTheme)
    {
        case "alcohol":
            additionalListLabels[0].classList.add(themeList[themeSelector.value]["alcohol-date"]["class"]);
            additionalListInput[0].value = themeList[themeSelector.value]["alcohol-date"]["value"];
            
            additionalListLabels[1].classList.add(themeList[themeSelector.value]["alcohol-brand"]["class"]);
            additionalListInput[1].value = themeList[themeSelector.value]["alcohol-brand"]["value"];
            break;
        case "books":
            additionalListLabels[0].classList.add(themeList[themeSelector.value]["books-date"]["class"]);
            additionalListInput[0].value = themeList[themeSelector.value]["books-date"]["value"];

            additionalListLabels[1].classList.add(themeList[themeSelector.value]["books-author"]["class"]);
            additionalListInput[1].value = themeList[themeSelector.value]["books-author"]["value"];
            break;
        case "films":
            additionalListLabels[0].classList.add(themeList[themeSelector.value]["films-date"]["class"]);
            additionalListInput[0].value = themeList[themeSelector.value]["films-date"]["value"];

            additionalListLabels[1].classList.add(themeList[themeSelector.value]["films-author"]["class"]);
            additionalListInput[1].value = themeList[themeSelector.value]["films-author"]["value"];
            break;
    }

    additionalListLabels[2].classList.add("lng-CollectionComments");
    additionalListInput[2].value = "CollectionComments";
    
    changeLanguage();
}    