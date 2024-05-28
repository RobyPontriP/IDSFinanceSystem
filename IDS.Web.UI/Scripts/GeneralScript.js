function NoAccess() {
    alert("You dont have access to create new country. Please contact your administrator.");
    return false;
}

//const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
//                        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
//];

function InputNumber(e) {
    $("#" + e.id).keyup(function (event) {
        if (event.which >= 37 && event.which <= 40) return;
        $(this).val(function (index, value) {
            return value
            // Keep only digits and decimal points:
            .replace(/[^\d.-]/g, "")
            // Remove duplicated decimal point, if one exists:
            .replace(/^(\d*\.)(.*)\.(.*)$/, '$1$2$3')
            // Keep only two digits past the decimal point:
            .replace(/\.(\d{2})\d+/, '.$1')
            // Add thousands separators:
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        });
    });
}

function RemoveAllComma(x) {
    if (x != null && x !== 'undefined') {
        return x.replaceAll(",", "");
    }
}

// Date Format MM/dd/yyyy
function AddYear(date, value) {
    var eDate = new Date(date);
    var eYear = parseFloat(eDate.getFullYear()) + parseFloat(value);
    var eFullDate = new Date((eDate.getMonth() + 1).toString() + "/" + eDate.getDate().toString() + "/" + eYear.toString());
}

function InputNumberMinus(e) {
    $("#" + e.id).keyup(function (event) {
        //if (event.which >= 37 && event.which <= 40) return;
        if (event.which >= 37 && event.which <= 40 && event.which != 46 && event.which != 45 && event.which != 46 &&
  !(event.which >= 48 && event.which <= 57)) return;
        $(this).val(function (index, value) {
            return value
            // Keep only digits and decimal points:
            .replace(/[^\d.-]/g, "")
            // Remove duplicated decimal point, if one exists:
            .replace(/^(\d*\.)(.*)\.(.*)$/, '$1$2$3')
            // Keep only two digits past the decimal point:
            .replace(/\.(\d{2})\d+/, '.$1')
            // Add thousands separators:
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
        });
    });
}



//Untuk input hanya angka di event keypress
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function AddCommaDecimal(numb) {
   return numb.toString().replace(/[^\d.-]/g, "")
                // Remove duplicated decimal point, if one exists:
                .replace(/^(\d*\.)(.*)\.(.*)$/, '$1$2$3')
                // Keep only two digits past the decimal point:
                .replace(/\.(\d{2})\d+/, '.$1')
                // Add thousands separators:
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

//
function GetPeriod(period) {
  //  const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  //"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
  //  ];

  //  period = monthNames[parseInt(period.substring(4, 6))] + " " + new Date().getFullYear();

  //  alert(period);

    return period;
}