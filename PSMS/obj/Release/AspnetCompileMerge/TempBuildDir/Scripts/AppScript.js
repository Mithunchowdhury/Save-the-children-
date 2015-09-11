
var iDiv; var innerDiv2; var isCreated;
var sec = 7;

function showMessage(code, msg, msgType) {
    if (isCreated) {
        hideMessage();
    }
    //if (!isCreated) {
        iDiv = document.createElement('div');
        iDiv.id = 'divErrorMsg';
        iDiv.className = 'errorBox';

        if (msgType == 0)
            iDiv.innerHTML = '<div style="margin-left:20px;background-image:url(\'images/warning.png\');background-repeat:no-repeat;float:left;width:70px;height:70px;background-size:contain;">&nbsp;<br /><br /><br /></div>' +
                        '<div style="margin-left: 30px;top:-10px;background-color:transparent;float:left;width:50%;"><b> ERROR - ' + code + ' : </b><br />' + msg + '</div>';
        else
            iDiv.innerHTML = '<div style="margin-left:20px;background-image:url(\'images/information.png\');background-repeat:no-repeat;float:left;width:70px;height:70px;background-size:contain;">&nbsp;<br /><br /><br /></div>' +
                    '<div style="margin-left: 30px;top:-10px;background-color:transparent;float:left;width:50%;"><b> INFO - ' + code + ' : </b><br />' + msg + '</div>';

        //if (msgType == 0)
        //    iDiv.innerHTML = '<b> ERROR - ' + code + ' : </b><br />' + msg;
        //else
        //    iDiv.innerHTML = '<b> SUCCESSFUL: </b><br />' + msg;

        document.getElementsByTagName('body')[0].appendChild(iDiv);
        if (msgType == 0)
            iDiv.setAttribute("style", "width:" + (screen.width - 10) + "px;background-color: red; border-top: 5px solid #c00;");
        else
            iDiv.setAttribute("style", "width:" + (screen.width - 10) + "px;background-color: green; border-top: 5px solid Blue;");

        iDiv.style.display = 'block';

        innerDiv2 = document.createElement('div');
        innerDiv2.id = 'divErrorClose';
        innerDiv2.className = 'errorBoxRight';        
        innerDiv2.innerHTML = 'X';
        innerDiv2.setAttribute("style", "height:30px");
        innerDiv2.style.visibility = 'visible';
        innerDiv2.addEventListener("click", hideMessage);
        iDiv.appendChild(innerDiv2);
        isCreated = true;

        hideError();
    //}
}

function hideError() {
    setTimeout(function () {
        hideMessage();
    }, sec * 1000);
}

function hideMessage() {
    try{
        var div = document.getElementById("divErrorMsg");
        div.parentNode.removeChild(div);
    }
    catch(er)
    {
       //try..catch for unwnated break occuring.
    }
    isCreated = false;
}


$(function () {
    $('.scimenu ul li').hover(
        function () {
            //show its submenu
            $('ul', this).slideDown(0);
        },
        function () {
            //hide its submenu
            $('ul', this).slideUp(0);
        }
    );

    $('nav ul li').hover(
    function () {
        //show its submenu
        $('ul', this).slideDown(0);
    },
    function () {
        //hide its submenu
        $('ul', this).slideUp(0);
    }
);
});


/*Ajax Loader - Starts*/
var totalSeconds = 0;
var countClock;

$(window).load(function () {

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(afterAJAXLoad);
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beforeAJAXLoad);
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(afterPageLoad);
});
function afterAJAXLoad() {
    try {

        clearInterval(countClock);
        totalSeconds = 0;
    } catch (err) { }
}

function beforeAJAXLoad(sender, args) {

    var altVal = '';
    try {
        altVal = args.get_postBackElement().alt;
        if (altVal == 'ajax') {
            var _divPageBase = document.getElementById('divPageBase');
            var _divLodingBase = document.getElementById('divLodingBase');
            var _divLodingSpin = document.getElementById('divLodingSpin');
            _divLodingSpin.style.display = 'block';
            divLodingBase.style.left = '0px'; //_divPageBase.offsetLeft + 1 + 'px';
            _divLodingBase.style.top = '0px';
            _divLodingBase.style.width = window.innerWidth + 'px'; //_divPageBase.offsetWidth + 'px'; //'894px';
            _divLodingBase.style.height = window.innerHeight + 'px'; //_divPageBase.offsetHeight + 99 + 'px';

            _divLodingSpin.style.left = _divPageBase.offsetLeft + 1 + 'px';
            _divLodingSpin.style.top = '0px';
            _divLodingSpin.style.width = _divPageBase.offsetWidth + 'px';  //'858px';
            countClock = setInterval(setTime, 1000);
        }
    }
    catch (err) {
        alert(err);
    }
}
function setTime() {

    ++totalSeconds;
    document.getElementById("seconds").innerHTML = pad(totalSeconds % 60);
    document.getElementById("minutes").innerHTML = pad(parseInt(totalSeconds / 60));
}
function pad(val) { if (val <= 9) return "0" + val; else return val; }

function afterPageLoad(sender, args) { }
/*Ajax Loader - Ends*/
