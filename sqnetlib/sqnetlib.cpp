//
// Squirrel.NET C++ library
// Handles print functions
//

#include <stdio.h>
#include <stdarg.h>
#include <squirrel.h>
#include <unordered_map>

#ifdef _MSC_VER
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

#ifdef SQUNICODE
#define scfprintf fwprintf
#define scvprintf vfwprintf
#define scvsprintf vswprintf
#else
#define scfprintf fprintf
#define scvprintf vfprintf
#define scvsprintf vsnprintf
#endif

typedef void(*SQNETPRINTFUNC)(HSQUIRRELVM, SQChar const *);

static std::unordered_map<HSQUIRRELVM, SQNETPRINTFUNC> printfuncs;
static std::unordered_map<HSQUIRRELVM, SQNETPRINTFUNC> errorfuncs;

extern "C" void sqnet_print(HSQUIRRELVM v, SQChar const *s, ...)
{
    // get buffer length
    va_list vl;
    va_start(vl, s);
    int len = scvsprintf(nullptr, 0, s, vl);
    va_end(vl);

    // create buffer
    SQChar *buffer = new SQChar[len + 1];
    va_start(vl, s);
    scvsprintf(buffer, len + 1, s, vl);
    va_end(vl);
	
	// call print function and free buffer
    printfuncs[v](v, buffer);
    delete[] buffer;
}

extern "C" void sqnet_error(HSQUIRRELVM v, SQChar const *s, ...)
{
    // get buffer length
    va_list vl;
    va_start(vl, s);
    int len = scvsprintf(nullptr, 0, s, vl);
    va_end(vl);

    // create buffer
    SQChar *buffer = new SQChar[len + 1];
    va_start(vl, s);
    scvsprintf(buffer, len + 1, s, vl);
    va_end(vl);
	
	// call print function and free buffer
    errorfuncs[v](v, buffer);
    delete[] buffer;
}

extern "C" EXPORT void sqnet_setprintfunc(HSQUIRRELVM v, SQNETPRINTFUNC printfunc, SQNETPRINTFUNC errfunc)
{
    sq_setprintfunc(v, sqnet_print, sqnet_error);
    printfuncs[v] = printfunc;
    errorfuncs[v] = errfunc;
}

extern "C" EXPORT SQNETPRINTFUNC sqnet_getprintfunc(HSQUIRRELVM v)
{
    return printfuncs[v];
}

extern "C" EXPORT SQNETPRINTFUNC sqnet_geterrorfunc(HSQUIRRELVM v)
{
    return errorfuncs[v];
}
