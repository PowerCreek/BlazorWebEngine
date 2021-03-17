console.log("loaded")

function SetParent(parentSelector, childSelector){
    document.getElementById(parentSelector).appendChild(document.getElementById(childSelector))
}