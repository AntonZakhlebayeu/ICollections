const keys = ["alcohol", "books", "films"];
const classKeys = ["date", "brand"];

const themeList = {
    "alcohol": {
        "date": {
            "class": "lng-alcoholDate",
        },
        "brand": {
            "class": "lng-alcoholBrand",
        },
    },
    
    "books": {
        "date": {
            "class": "lng-booksDate",
        },
        "brand": {
            "class": "lng-booksAuthor",
        }
    },
    
    "films": {
        "date": {
            "class": "lng-filmsDate",
        },
        "brand": {
            "class": "lng-filmsAuthor",
        }
    }
    
}

const themeSelector = document.getElementById("themeSelector");

themeSelector.addEventListener("change", changeContext)

function changeContext() {
    let selectedTheme = themeSelector.value;
    
    let additionalVision = document.getElementById("checkBoxAdditional").style;
    
    if (selectedTheme === "null") {
        additionalVision.display = "none";      
        return;
    }
    
    additionalVision.display = "block";
    let additionalListLabels = document.getElementsByClassName("form-check-label");

    changeClass(additionalListLabels, selectedTheme);
    
    changeLanguage();
}    

function changeClass(additionalListLabels, selectedTheme) {
    for(let i = 0; i < 2; i++)
        for(let key in keys)
            for(let classKey in classKeys) 
                if(additionalListLabels[i].classList.contains(themeList[keys[key]][classKeys[classKey]]["class"]))
                    additionalListLabels[i].classList.remove(themeList[keys[key]][classKeys[classKey]]["class"]);
            

    for(let i = 0; i < 2; i++) 
        additionalListLabels[i].classList.add(themeList[selectedTheme][classKeys[i]]["class"]);
    
    additionalListLabels[2].classList.add("lng-CollectionComments");
}

changeContext();changeContext();

