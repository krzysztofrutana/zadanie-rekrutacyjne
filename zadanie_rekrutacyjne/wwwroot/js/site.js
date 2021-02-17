// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.


/// CHANGE COLLAPSE PANEL ICON WHEN CLICK CATEGORY BUTTON 
function collapsePanelIconChange(elem) {
    var parentDiv = elem.parentNode.parentNode;
    var children = parentDiv.children[0];
    console.log($(children).hasClass('fa-chevron-right'));
    if ($(children).hasClass('fa-chevron-right')) {
        $(children).removeClass('fa-chevron-right');
        $(children).addClass('fa-chevron-down');
        return true;
    }
    if ($(children).hasClass('fa-chevron-down')) {
        $(children).removeClass('fa-chevron-down');
        $(children).addClass('fa-chevron-right');
        return true;
    }
};

/// SHOW/HIDE ALL PANEL AND CHANGE ICON WHEN CLICK SHOW ALL BUTTON
function reverseCollapseState() {
    // get all containers, and all element with current icon
    var containers = $('.collapse-container');
    var arrowsDown = $('.fa-chevron-down');
    var arrowsRight = $('.fa-chevron-right');

    // counter how many is show or hidden
    var howManyIsShow = 0;
    var howManyIsCollapsed = 0;

    // check all containers and count 
    for (var i = 0; i < containers.length; i++) {
        if ($(containers[i]).hasClass('show')) {
            howManyIsShow = howManyIsShow + 1;
        } else {
            howManyIsCollapsed = howManyIsCollapsed + 1;
        }
    }
    // if all containers are shwo or hidden reverse state
    if (howManyIsShow == containers.length || howManyIsCollapsed == containers.length) {
        if ($('.collapse-container').hasClass('show')) {
            $('.collapse-container').removeClass('show');
            for (var i = 0; i < arrowsDown.length; i++) {
                // tree-sort is class added to sort button in tree (used this same fa-chevron icon)
                if (!($(arrowsDown[i]).hasClass('tree-sort'))) {
                    $(arrowsDown[i]).removeClass('fa-chevron-down');
                    $(arrowsDown[i]).addClass('fa-chevron-right');
                }
            }
        } else {
            $('.collapse-container').addClass('show');
            for (var i = 0; i < arrowsRight.length; i++) {
                $(arrowsRight[i]).removeClass('fa-chevron-right');
                $(arrowsRight[i]).addClass('fa-chevron-down');
            }
        }
        // if hidden containers is equal or more then hidden everything
    } else if (howManyIsCollapsed >= howManyIsShow) {
        for (var i = 0; i < containers.length; i++) {
            if ($(containers[i]).hasClass('show')) {
                $(containers[i]).removeClass('show');
            }
        }
        for (var i = 0; i < arrowsDown.length; i++) {
            if (!($(arrowsDown[i]).hasClass('tree-sort'))) {
                $(arrowsDown[i]).removeClass('fa-chevron-down');
                $(arrowsDown[i]).addClass('fa-chevron-right');
            }
        }
        // if shown containers is  more then show everything
    } else if (howManyIsCollapsed < howManyIsShow) {
        for (var i = 0; i < containers.length; i++) {
            if (!$(containers[i]).hasClass('show')) {
                $(containers[i]).addClass('show');
            }
        }
        for (var i = 0; i < arrowsRight.length; i++) {
            $(arrowsRight[i]).removeClass('fa-chevron-right');
            $(arrowsRight[i]).addClass('fa-chevron-down');
        }
    };
};



// SEND INFORMATION ABOTU COLLAPSE CONTAINER STATE (SEND IF SHOWN) IF COLLAPSE CONTAINER IS SHOWN
$(".panel .collapse-container").on('shown.bs.collapse', function () {
    var active = $(this).attr('id'); // get id of current panel
    var panels = localStorage.panels === undefined ? new Array() : JSON.parse(localStorage.panels); // if localstorage is empty create new array or if not load localstorage
    if ($.inArray(active, panels) == -1) //check that the element is not in the array
        panels.push(active); // if not add id to array
    localStorage.panels = JSON.stringify(panels); // save array to local storage

    // send array to controller method which save information in singleton service
    var JsonLocalStorageObj = JSON.stringify(localStorage);
    $.ajax({
        url: 'SetShownPanel',
        type: "POST",
        data: { JsonLocalStorageObj: JsonLocalStorageObj },
        success: function (result) {
            console.log('LocalStorage sended')
        }
    });
    return false;
});


// SEND INFORMATION ABOTU COLLAPSE CONTAINER STATE (SEND IF HIDDEN) IF COLLAPSE CONTAINER IS HIDDEN
$(".panel .collapse-container").on('hidden.bs.collapse', function () {
    // method this same as above, except that this one removes the element
    var active = $(this).attr('id');
    var panels = localStorage.panels === undefined ? new Array() : JSON.parse(localStorage.panels);
    var elementIndex = $.inArray(active, panels);
    if (elementIndex !== -1) 
    {
        panels.splice(elementIndex, 1); //remove item from array        
    }
    localStorage.panels = JSON.stringify(panels); 
    var JsonLocalStorageObj = JSON.stringify(localStorage);

    $.ajax({
        url: 'SetShownPanel',
        type: 'POST',
        data: { JsonLocalStorageObj: JsonLocalStorageObj },
        success: function (result) {
            console.log('LocalStorage sended')
        }
    });

    return false;
});
