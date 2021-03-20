console.log("loaded")

function SetParent(parentSelector, childSelector){
    document.getElementById(parentSelector).appendChild(document.getElementById(childSelector))
}

function SetStyles(elementId, styleKey, styleValue){
    document.getElementById(elementId).style[styleKey]=styleValue
}