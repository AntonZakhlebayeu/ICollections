const keys = ["alcohol", "books", "brand"];
const classKeys = ["date", "brand"];

const themeList = {
    "alcohol": {
        "date": {
            "class": "lng-alcoholDate",
            "value": "alcoholDate",
        },
        "brand": {
            "class": "lng-alcoholBrand",
            "value": "alcoholBrand",
        },
    },
    
    "books": {
        "date": {
            "class": "lng-booksDate",
            "value": "booksDate",
        },
        "brand": {
            "class": "lng-booksAuthor",
            "value": "booksAuthor"
        }
    },
    
    "films": {
        "date": {
            "class": "lng-filmsDate",
            "value": "filmsDate",
        },
        "brand": {
            "class": "lng-filmsAuthor",
            "value": "filmsAuthor",
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

    changeClass(additionalListLabels, selectedTheme);
    changeValue(additionalListInput, selectedTheme);
    
    changeLanguage();
}    

function changeClass(additionalListLabels, selectedTheme) {
    for(let additionalListLabel in additionalListLabels)
        for(var key in keys)
            for(var classKey in classKeys) 
                if(additionalListLabels[index].classList.contains(themeList[key][classKey]["class"]))
                    additionalListLabel.classList.remove(themeList[key][classKey]["class"])

    for(var i = 0; i < 2; i++) 
        additionalListLabels[i].classList.add(themeList[selectedTheme][i]["class"])
    
    additionalListLabels[2].classList.add("lng-CollectionComments");
}

function changeValue(additionalListInputs, selectedTheme) {
    for(var i = 0; i < 2; i++)
        additionalListInputs[i].value = (themeList[selectedTheme][i]["value"])

    additionalListInputs[2].value = "CollectionComments";
}
