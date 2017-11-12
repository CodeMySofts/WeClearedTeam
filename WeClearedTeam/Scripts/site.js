(function (funcName, baseObj) {
    // The public function name defaults to window.docReady
    // but you can pass in your own object and own function name and those will be used
    // if you want to put them in a different namespace
    funcName = funcName || "docReady";
    baseObj = baseObj || window;
    var readyList = [];
    var readyFired = false;
    var readyEventHandlersInstalled = false;

    // call this when the document is ready
    // this function protects itself against being called more than once
    function ready() {
        if (!readyFired) {
            // this must be set to true before we start calling callbacks
            readyFired = true;
            for (var i = 0; i < readyList.length; i++) {
                // if a callback here happens to add new ready handlers,
                // the docReady() function will see that it already fired
                // and will schedule the callback to run right after
                // this event loop finishes so all handlers will still execute
                // in order and no new ones will be added to the readyList
                // while we are processing the list
                readyList[i].fn.call(window, readyList[i].ctx);
            }
            // allow any closures held by these functions to free
            readyList = [];
        }
    }

    function readyStateChange() {
        if (document.readyState === "complete") {
            ready();
        }
    }

    // This is the one public interface
    // docReady(fn, context);
    // the context argument is optional - if present, it will be passed
    // as an argument to the callback
    baseObj[funcName] = function (callback, context) {
        if (typeof callback !== "function") {
            throw new TypeError("callback for docReady(fn) must be a function");
        }
        // if ready has already fired, then just schedule the callback
        // to fire asynchronously, but right away
        if (readyFired) {
            setTimeout(function () { callback(context); }, 1);
            return;
        } else {
            // add the function and context to the list
            readyList.push({ fn: callback, ctx: context });
        }
        // if document already ready to go, schedule the ready function to run
        if (document.readyState === "complete") {
            setTimeout(ready, 1);
        } else if (!readyEventHandlersInstalled) {
            // otherwise if we don't have event handlers installed, install them
            if (document.addEventListener) {
                // first choice is DOMContentLoaded event
                document.addEventListener("DOMContentLoaded", ready, false);
                // backup is window load event
                window.addEventListener("load", ready, false);
            } else {
                // must be IE
                document.attachEvent("onreadystatechange", readyStateChange);
                window.attachEvent("onload", ready);
            }
            readyEventHandlersInstalled = true;
        }
    }
})("docReady", window);

// Table qui contien les index pour chacune des classes et spécialisations
var classesSpecs = {
    "Death Knight": { "Blood": 8, "Frost": 16, "Unholy": 27 },
    "Demon Hunter": { "Havoc": 19, "Vengeance": 31 },
    "Druid": { "Balance": 5, "Feral": 14, "Guardian": 18, "Restoration": 25 },
    "Hunter": { "Beast Mastery": 6, "Marksmanship": 21, "Survival": 30 },
    "Mage": { "Arcane": 2, "Fire": 15, "Frost": 16 },
    "Monk": { "Brewmaster": 7, "Mistweaver": 22, "Windwalker": 32 },
    "Paladin": { "Holy": 20, "Protection": 24, "Retribution": 26 },
    "Priest": { "Discipline": 11, "Holy": 20, "Shadow": 28 },
    "Rogue": { "Assassination": 4, "Outlaw": 23, "Subtlety": 29 },
    "Shaman": { "Elemental": 12, "Enhancement": 13, "Restoration": 25 },
    "Warlock": { "Affliction": 1, "Demonology": 9, "Destruction": 10 },
    "Warrior": { "Arms": 3, "Fury": 17, "Protection": 24 }
};

// ajouter des evénements contrôlés par jQuery, seulement quand le document est chargé.
docReady(function () {
    // Fonction qui permet d'ajouter seulement les spécialisations valides lors de la sélection de classe.
    function changerOptions(selectedText, isLoad) {
        // Continuer seulement si on à une classe valide
        if (typeof (selectedText) === "string") {
            // Mémoriser le spec pour pouvoir le rétablir si l'appel vien d'Edit
            var specText = $("#SpecValue").val();
            // Vider le select
            $("#Specialisation").empty();
            // table des options qui devront être ajoutées dans le select
            var options = [];
            // itérer la table des classes et créer les option dont nous aurons besoin
            $.each(classesSpecs[selectedText],
                function (index, value) {
                    // forcer la sélection si on provien du load.
                    if (isLoad && index === specText) {
                        var option = $("<option></option>").attr("value", value).text(index);
                        option.attr("selected", true);
                        options[options.length] = option;
                    }
                    // ajouter les options
                    else {
                        options[options.length] = $("<option></option>").attr("value", value).text(index);
                    }
                });
            // appliquer les options générées dans le select
            $("#Specialisation").append(options);
        }
    }
    // lancer la fonction changerOptions quand la classe est changée
    $("#Classe").change(function () {
        changerOptions($("#Classe option:selected").text());
    });
    // lancer la fonction changerOptions une fois au chargement
    changerOptions($("#Classe option:selected").text(), true);
});