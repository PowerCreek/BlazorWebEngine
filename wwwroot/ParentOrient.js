console.log("loaded")

function SetParent(parentSelector, childSelector){
    document.getElementById(parentSelector).appendChild(document.getElementById(childSelector))
}

function SetStyles(elementId, styleKey, styleValue){
    document.getElementById(elementId).style[styleKey]=styleValue
}

function SetStylesByReference(element, styleKey, styleValue){
    console.log(styleKey[0])
    element.style[styleKey]=styleValue
}