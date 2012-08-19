//tty_wrap
exports.guessHandleType = function(id) {
    if (id == 1)
        return 'TTY';
    return '-tty_wrap-';
}
