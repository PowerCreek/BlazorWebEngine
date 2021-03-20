console.log("loaded")

function SetParent(parentSelector, childSelector){
    document.getElementById(parentSelector).appendChild(document.getElementById(childSelector))
}

function SetStyles(elementId, styleKey, styleValue){
    console.log(elementId)
    console.log(styleKey)
    console.log(styleValue)
}