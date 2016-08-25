// site.js

// CAN STILL HAVE GLOBAL SCOPE COLLISION USING startup FUNCTION!!!
//function startup() { 
//    // Populate username
//    var ele = document.getElementById("username");
//    ele.innerHTML = "Uncle Gary";

//    // Wire-up event handlers
//    var main = document.getElementById("main");

//    main.onmouseover = function () {
//        main.style.backgroundColor = "#888";
//        //main.style = "background:#888;"; Does not work for all browsers!
//    };

//    main.onmouseleave = function () {
//        main.style.backgroundColor = "";
//        //main.style = ""; Does not work for all browsers!
//    }
//}

//startup();

// Use Self Executing Anonymous Functions also called Immediately Invoked Function Expressions instead!
(function () {
    // Populate username
    //var ele = document.getElementById("username");  Use jQuery instead!
    //ele.innerHTML = "Uncle Gary";

    var ele = $("#username"); // Using jQuery!
    ele.text("Uncle Gary");

    //// Wire-up event handlers USE jQuery INSTEAD!!!
    //var main = document.getElementById("main");

    //main.onmouseover = function () {
    //    main.style.backgroundColor = "#888";
    //    //main.style = "background:#888;"; Does not work for all browsers!
    //};

    //main.onmouseleave = function () {
    //    main.style.backgroundColor = "";
    //    //main.style = ""; Does not work for all browsers!
    //};

    // Wire-up event handlers using jQuery instead:
    var main = $("#main");
    
    //main.on("mouseenter", function () { <- REMOVED TO START USING BOOTSTRAP!!!
    //    //main.css.style = "background:#888;"; This does not work!
    //    $(this).css("background-color", "#888")
    //});

    //main.on("mouseleave", function () {
    //    //main.style = ""; This does not work
    //    $(this).css("background-color", "");
    //});

    // Wire-up the menu items
    var menuItems = $("ul.menu li a");
    menuItems.on("click", function () {
        //var me = $(this); NOW ACTUALLY ROUTING!
        //alert(me.text());
    });

    // Wire sidebar toggle functionality
    var $sidebarAndWrapper = $("#sidebar, #wrapper");
    var $icon = $("#sidebarToggle i.fa"); // <- NOW USING BOOTSTRAP!!!

    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");

        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            //$(this).text("Show Sidebar") <- NOW USING BOOTSTRAP!!!
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        }
        else {
            //$(this).text("Hide Sidebar"); <- NOW USING BOOTSTRAP!!!
            $icon.removeClass("fa-angle-right");
            $icon.addClass("fa-angle-left");
        }
    });

})();

