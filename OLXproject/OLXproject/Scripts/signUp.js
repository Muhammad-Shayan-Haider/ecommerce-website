$(function() {

$("#password_error_message").hide();
$("#email_error_message").hide();
$("#password_notMatch_message").hide();
$("#city_error_message").hide();

let error_password = false;
let error_email = false;
let error_city = false;
let error_misspassword = false;

  $("#form_email").keyup(function() {

    check_email();

  });

$("#form_password").keyup(function() {

check_password();

});

$("#form_recheckpassword").keyup(function() {

check_recheckpassword();

});

$("#form_city").keyup(function() {

check_city();

});


function check_password() {

let password_length = $("#form_password").val().length;
if(password_length < 8) {
$("#password_error_message").html("Password must be 8 characters long").addClass("error");
      $("#password_error_message").show();
      $("#form_password").css({border: "3px solid red"});

error_password = true;
} else {
      $("#form_password").css({border: "3px solid #36cc36"});
$("#password_error_message").hide();
}

}


function check_recheckpassword() {

let password = $("#form_password").val();
let reenterpassword = $("#form_recheckpassword").val();
if(password != reenterpassword) {
$("#password_noMatch_message").html("Passwords are not similar").addClass("error");
      $("#password_noMatch_message").show();
      $("#form_recheckpassword").css({border: "3px solid red"});

error_misspassword = true;
} else {
      $("#form_recheckpassword").css({border: "3px solid #36cc36"});
$("#password_noMatch_message").hide();
}

}


function check_email() {

var pattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i);

if(pattern.test($("#form_email").val())) {
      $("#form_email").css({border: "3px solid #36cc36"});
$("#email_error_message").hide();
} else {
$("#email_error_message").html("Invalid email address").addClass("error");
      $("#form_email").css({border: "3px solid red"});
$("#email_error_message").show();
error_email = true;
}

}

function check_city() {

let pattern= /^[a-zA-Z]+$/i;

if(pattern.test($("#form_city").val())) {
      $("#form_city").css({border: "3px solid #36cc36"});
$("#city_error_message").hide();
} else {
$("#city_error_message").html("City can only contain alphabets").addClass("error");
      $("#form_city").css({border: "3px solid red"});
$("#city_error_message").show();
city_email = true;
}

}

$("#myform").submit(function() {

error_password = false;
error_email = false;
error_city = false;
error_misspassword = false;

check_password();
check_email();
check_recheckpassword();
check_city();

if(error_password == false && error_email == false && error_city == false && error_misspassword == false) {
return true;
} else {
return false;
}

});

});

