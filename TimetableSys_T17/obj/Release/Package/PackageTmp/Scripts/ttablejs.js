var parks_container = [];
var buildings_container = [];
var rooms_container = [];
var facilities_container = [];
var module_code_container = [];
var module_title_container = [];
var session_type_container = [];
var selected_weeks_container = [];



/********************************************
KNOWN BUGS - ADAM 

WHEN UNSELECT OPTION, WHEN FILTERED, CLEAR SELECTED LIST, AS VALUES STILL EXIST

*********************************************/

$(document).ready(function () {
    $.fn.bootstrapBtn = $.fn.button.noConflict(); // To remedy Bootstrap - jQueryUI conflict
    $("#parks_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select a Park";

            }
            else
            {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else
                    {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function (event) {

            if ($("#parks_input").children("option").length == 0) {

                AjaxCall(1);
            }

        },
        onChange: function (option, checked, select) {

            parks_container = arr_builder($(option).val(), checked, parks_container);
            AjaxCall(5);

        }

    });

    $("#buildings_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select a Building";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {
         
            if ($("#buildings_input").children("option").length == 0) {

                AjaxCall(2);
            }

        },
        onChange: function (option, checked, select) {

            buildings_container = arr_builder($(option).val(), checked, buildings_container);
            AjaxCall(6);

        }
    });

    $("#rooms_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select Rooms";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {

            if ($("#rooms_input").children("option").length == 0) {

                AjaxCall(3);
            }

        },
        onChange: function (option, checked, select) {

            rooms_container = arr_builder($(option).val(), checked, rooms_container);
            AjaxCall(7);
            drawTable();
        }
    });

    $("#facilities_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select Facilities";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {

            if ($("#facilities_input").children("option").length == 0) {

                AjaxCall(4);
            }

        },
        onChange: function (option, checked, select) {

            facilities_container = arr_builder($(option).val(), checked, facilities_container);
            AjaxCall(8);

        }
    });

    $("#module_code_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select a Module Code";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {

            if ($("#module_code_input").children("option").length == 0) {

                AjaxCall(9);
            }

        },
        onChange: function (option, checked, select) {

            module_code_container = arr_builder($(option).val(), checked, module_code_container);
            AjaxCall(10);
            drawTable();

        }
    });

    $('#module-code-input').on('click', function () {
        $('#module_code_input').multiselect('deselectAll', false);
        $('#module_code_input').multiselect('updateButtonText');
    });

    $("#module_title_input").multiselect({ 

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select a Module Title";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {

            if ($("#module_title_input").children("option").length == 0) {

                AjaxCall(11);
            }

        },
        onChange: function (option, checked, select) {

            module_title_container = arr_builder($(option).val(), checked, module_title_container);
            AjaxCall(12);
            drawTable();

        }
    });

    $("#session_type_input").multiselect({

        enableFiltering: true,
        maxHeight: 250,
        buttonWidth: 250,
        buttonText: function (options, select) {

            if (options.length == 0) {

                return "Select a Session Type";

            }
            else {

                var labels = [];

                options.each(function () {

                    if ($(this).attr('label') !== undefined) {

                        labels.push($(this).attr('label'));

                    }
                    else {

                        labels.push($(this).html());

                    }
                });

                return labels.join(', ') + '';
            }
        },
        onDropdownShow: function () {

            if ($("#session_type_input").children("option").length == 0) {

                AjaxCall(13);
                
            }

        },
        onChange: function (option, checked, select) {

            session_type_container = arr_builder($(option).val(), checked, session_type_container);
            drawTable();
        }
    });

    $(function () {

        $("#special_requirements").resizable({

            animate: true,
            animateDuration: "slow",
            animateEasing: "easeOutBounce",
            delay: 150,
            minHeight: 200,
            minWidth: 500,
            maxHeight: 400,
            maxWidth: 700,
            handles: "se"
        });

    });

    $(function () {

        $("#weeks_container").buttonset().change(function () {

            selected_weeks_container = [];

            $.each($("input[name='weeks']:checked"), function () {


                selected_weeks_container.push(parseInt($("label[for = " + $(this).attr("id") + "]").text()));

            });

            drawTable();
        });
    });
});

function drawTable() {
    
    if (module_code_container.length > 0 && module_title_container.length > 0 && session_type_container.length > 0 && selected_weeks_container.length > 0) {

        if (rooms_container.length > 0) {
            
            AjaxCall(14)

            //if rooms > 0 then can obtain facilities desired

        } else if (rooms_container.length == 0 && facilities_container.length > 0) {

            //AjaxCall(15)

            //we can setup request with facilities and no room - admin can use judgement

        } else {

            //AjaxCall(16)

            // else doesn't matter for draw table - when submit, check park & building, if 1 .length > 0 add to roomCol. 
        }


    }

}
var room_i_spinner = $("#room_i").spinner({ min: 1 });
var room_ii_spinner = $("#room_ii").spinner({ min: 1 });
var room_iii_spinner = $("#room_iii").spinner({ min: 1 });



function arr_builder(val, checked, array) {

    
    if (!checked) {

        array.splice(array.indexOf(val), 1);

    } else {

        array.push(val);

    }

    return array;

}

function dropDownConstructor(input, recieved_data, target) {

    placeholder = [];
    if (input.length != 0) {

        recieved_data.forEach(function (value) {

            if (input.indexOf(value) != -1) {

                var stepping = { label: value, title: value, value: value, selected: true }
                placeholder.push(stepping);


            } else {

                var stepping = { label: value, title: value, value: value }
                placeholder.push(stepping);

            }
        });

    } else {
        
        recieved_data.forEach(function (value) {

            var stepping = { label: value, title: value, value: value }
            placeholder.push(stepping);

        });

    }

    $(target).multiselect("dataprovider", placeholder);

}

function AjaxCall(call) {

    place_holder = [];
    console.log("here");
    $.ajax({

        url: "Request/RequestModelUpdaterOptional2",
        type: "GET",
        data: {

            which_call: call,
            park_names: JSON.stringify(parks_container),
            building_names: JSON.stringify(buildings_container),
            room_names: JSON.stringify(rooms_container),
            facility_names: JSON.stringify(facilities_container),
            module_code: JSON.stringify(module_code_container),
            module_title: JSON.stringify(module_title_container),
            session_type: JSON.stringify(session_type_container)

        },
        contentType: "application/json",
        success: function (data) {
           
            if (data.parkName != null) { dropDownConstructor(parks_container, data.parkName, "#parks_input"); }
            if (data.buildingName != null) { dropDownConstructor(buildings_container, data.buildingName, "#buildings_input"); }
            if (data.roomCode != null) { dropDownConstructor(rooms_container, data.roomCode, "#rooms_input"); }
            if (data.facilities != null) { dropDownConstructor(facilities_container, data.facilities, "#facilities_input"); }
            if (data.moduleCode != null) { dropDownConstructor(module_code_container, data.moduleCode, "#module_code_input"); }
            if (data.moduleTitle != null) { dropDownConstructor(module_title_container, data.moduleTitle, "#module_title_input"); }
            if (data.sessionType != null) { dropDownConstructor(session_type_container, data.sessionType, "#session_type_input"); }
            if (data.tableRequest == null) { data.tableRequest = []; populateTable(data.tableRequest); } else { populateTable(data.tableRequest); };


        }
    });
}

function populateTable(responseData) {

    var tableIDs = ["m", "t", "w", "h", "f"];
    var days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
    temp_container = []; // for update table;

    $("#timetable").empty();
    var parentDiv = document.createElement("div");
    $(parentDiv).attr("id", "timetable_table");
    
    for (var i = 0; i < 6; i++) {

        var dayDiv = document.createElement("div");
        $(dayDiv).attr("id", days[i-1]);

        for (var y = 0; y < 10; y++) {

            var dayTag = tableIDs[i - 1];
            var fullTag = dayTag + y.toString();

            if (y == 0 && i == 0) {

                var firstDiv = document.createElement("div");
                $(firstDiv).attr("id", "origin");
                $(firstDiv).attr("style", "width: 100px; height: 50px; background-color: orange; display: inline-block; color: white;");
                $(firstDiv).appendTo(dayDiv);

            } else if (y == 0 && i != 0) {

                var firstDiv = document.createElement("div");
                $(firstDiv).attr("id", "days");
                $(firstDiv).attr("style", "width: 100px; height: 50px; background-color: blue; display: inline-block; color: white;");
                $(firstDiv).html(days[i-1]);
                $(firstDiv).appendTo(dayDiv);

            } else if (y != 0 && i == 0) {
          
                var topDivs = document.createElement("div");
                $(topDivs).attr("id", "times");
                $(topDivs).attr("style", "width: 100px; height: 50px; background-color: red; display: inline-block; color: white;");
                $(topDivs).html((y + 8) + ":00 - " + (y + 9) + ":00");
                $(topDivs).appendTo(dayDiv);

            } else {

                var bodyDivs = document.createElement("div");
                $(bodyDivs).attr("id", fullTag);
                $(bodyDivs).attr("name", 0);
                $(bodyDivs).click(function () { updateRequestTable($(this).attr("id"), $(this).parent().attr("id"), temp_container, days); });
                $(bodyDivs).attr("style", "width: 100px; height: 50px; background-color: pink; display: inline-block;");
                $(bodyDivs).appendTo(dayDiv);

            }
        }

        $(dayDiv).appendTo(parentDiv);
    }

    $(parentDiv).appendTo($("#timetable"));

    responseData.forEach(function(value)
    {


        var day, moment; var collection = [];
        value.forEach(function (inception_minus_two) {
            
            if (returnValidTable(inception_minus_two.week, selected_weeks_container)) {

                switch (inception_minus_two.dayID) {

                    case 1: day = "m"; break;
                    case 2: day = "t"; break;
                    case 3: day = "w"; break;
                    case 4: day = "h"; break;
                    case 5: day = "f"; break;

                }

                moment = inception_minus_two.periodID + inception_minus_two.sessionLength;


                for (var i = 0; i < moment; i++) {

                $("#" + day + "" + i).html("Booked");
                $("#" + day + "" + i).attr("name", 3);

                }
                $("#" + day + "" + inception_minus_two.periodID).html("Booked");
                $("#" + day + "" + inception_minus_two.periodID).attr("name", 3);


            }
        });
    });
}


function returnValidTable(dbData, usrData) {



    dbData = (dbData.substring(1, dbData.length - 1)).split(",");

    var return_val = false;

    for (var i = 0; i < usrData.length; i++) {

        if (dbData[(usrData[i]-1)] == 1) {

            return_val = true;

            break;
        }

    }

    return return_val;

}

function updateRequestTable(input, input_parent, container, days) {

    ph = [];

    if ($("#" + input).attr("name") == 4) {

        if (($("#" + input[0] + (parseInt(input.match(/[0-9]*$/)) - 1)).attr("name") == 4 && ($("#" + input[0] + (parseInt(input.match(/[0-9]*$/)) + 1)).attr("name") == 4))) {



            alert("Can not fragment sessions!");

        } else {

            $("#" + input).attr("name", 1);
            $("#" + input).html("Available");

        }

    } else if ($("#" + input).attr("name") == 1) {


        counter_4 = 0;

        var flag = false;

        ph.push(parseInt($("#" + input).attr("id").match(/[0-9]*$/)));

        $("#" + input_parent).children().each(function () {

            if ($(this).attr("name") == 4) {

                ph.push(counter_4);
                ph = ph.sort();
               
                counter_4++;


            } else {

                counter_4++;
            }
        });

       
            if (ph.length >= 2) {

                for (var i = 0; i < (ph.length - 1) ; i++) {

                    if (Math.abs(ph[i + 1] - ph[i]) != 1) {

          
                        flag = true;
                        $(this).parent().attr("name", 1);
                        break;
                    } else {

                        flag = false;
                    }
                }
            } 
      

        if (flag) {

            alert("You can not fragment your session!");

        } else {

            $("#" + input).attr("name", 4);
            $("#" + input).html("Selected");

        }

    } else if ($("#" + input).attr("name") == 1 && $("#" + input[0] + (parseInt(input.match(/[0-9]*$/)) - 1)).attr("name") == 4) {

        $("#" + input).attr("name", 4);
        $("#" + input).html("Selected");
        $("#" + input[0] + (parseInt(input.match(/[0-9]*$/)) - 2)).attr("name", 2);
        $("#" + input[0] + (parseInt(input.match(/[0-9]*$/)) - 2)).html("DoC");

    } else if ($("#" + input).attr("name") == 2) {

        
          
        $("#" + $("div[name=4]").parent().attr("id")).children().each(function () {


            if ($(this).attr("name") == 4 || $(this).attr("name") == 1) {


                $(this).attr("name", 2);
                $(this).html("")

            }
        });

        $("#" + input_parent).children().each(function () {
           
            if ($(this).attr("id") == input && $(this).attr("name") == 2) {

                $(this).attr("name", 4);
                $(this).html("Selected");

            } else if ($(this).html() != input_parent && $(this).attr("name") == 2) {

                $(this).attr("name", 1);
                $(this).html("Available");

            }
        });

    }else{

        $("#" + input_parent).children().each(function () {

            if ($(this).attr("id") == input && $(this).attr("name") == 0) {

                $(this).attr("name", 4);
                $(this).html("Selected");

            } else if ($(this).html() != input_parent && $(this).attr("name") == 0) { //change this to class

                $(this).attr("name", 1);
                $(this).html("Available");

            } 

        });

        days.forEach(function (day) {

            if (day != input_parent) {

                $("#" + day).children().each(function () {

                    if ($(this).html() == "" && $(this).attr("name") == 0) {

                        $(this).html("");
                        $(this).attr("name", 2);
                    }
                });
            };
        });
    }
}

function Submit() {


    // ContructWeeks();
    // GetDay(); getTimes();
    $.ajax({


        url: "SubmitThisThing",
        type: "GET",
        data: {

            which_call: 25,
            park_names: JSON.stringify(parks_container),
            building_names: JSON.stringify(buildings_container),
            room_names: JSON.stringify(rooms_container),
            facility_names: JSON.stringify(facilities_container),
            module_code: JSON.stringify(module_code_container),
            module_title: JSON.stringify(module_title_container),
            session_type: JSON.stringify(session_type_container),
            weeks: JSON.stringify(selected_weeks_container),
            day: JSON.stringify(grabSelectedDay()),
            dayInfo: JSON.stringify(grabSelectedDays())

        },
        contentType: "application/json",
        success: function (response) {
           
            alert(response.response);
        }   
    });
}

function grabSelectedDay() {

    return $("div:contains(\"Selected\")")[4].id;

}

function grabSelectedDays() {

    var periodID = null;
    var sessionLength = 1;
    var sessionInfo = [];

    var input = grabSelectedDay();

    $("#" + input).children().each(function () {

        if ($(this).attr("name") == 4 && periodID == null) {

            periodID = parseInt($(this).attr("id").match(/[0-9]*$/));

        }else if ($(this).attr("name") == 4 && periodID != null) {

            sessionLength =  (parseInt($(this).attr("id").match(/[0-9]*$/)) - periodID) + 1;

        }
    });

    sessionInfo.push(periodID);
    sessionInfo.push(sessionLength);

    return sessionInfo;
}