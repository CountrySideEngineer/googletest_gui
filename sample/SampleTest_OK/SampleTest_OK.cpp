#include <Windows.h>
#include <tchar.h>
#include "CAdd.h"
#include "CMultiple.h"
#include "gtest/gtest.h"

/*
 *	Unit test of CAdd class method.
 */
TEST(CAdd_test, calc_001) {
	CAdd testTarget;

	int inA = 1;
	int inB = 2;
	int calcResult = testTarget.Calc(inA, inB);

	ASSERT_EQ(3, calcResult);
}

TEST(CAdd_test, calc_002) {
	CAdd testTarget;

	int inA = 0x7FFF;
	int inB = 0x7FFF;
	int calcResult = testTarget.Calc(inA, inB);

 	ASSERT_EQ(0xFFFE, calcResult);
}

/*
 *	Unit test of CMultiple class method.
 */
TEST(CMultiple_test, calc_001) {
	CMultiple testTarget;

	int inA = 1;
	int inB = 2;
	int calcResult = testTarget.Calc(inA, inB);

	ASSERT_EQ(2, calcResult);
}

TEST(CMultiple_test, calc_002) {
	CMultiple testTarget;

	int inA = 0x7FFF;
	int inB = 0x7FFF;
	int calcResult = testTarget.Calc(inA, inB);

	ASSERT_EQ(0x3FFF0001, calcResult);
}
