$(function() {

$("#password_error_message").hide();
$("#email_error_message").hide();

let error_password = false;
let error_email = false;

  $("#form_email").keyup(function() {

    check_email();

  });

$("#form_password").keyup(function() {

check_password();

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

$("#myform").submit(function() {

error_password = false;
error_email = false;

check_password();
check_email();

if(error_password == false && error_email == false) {
return true;
} else {
return false;
}

});

});