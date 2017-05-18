# Sq.NET Library

cmake_minimum_required(VERSION 3.4)
project(sqnetlib CXX)

add_subdirectory(squirrel)
include_directories(squirrel/include)

add_library(sqnetlib SHARED sqnetlib/sqnetlib.cpp)
target_link_libraries(sqnetlib squirrel)
