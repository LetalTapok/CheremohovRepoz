#include <iostream>
#include <string>
#include <cmath>
#include <clocale>
#include <windows.h>
using namespace std;


void enableRussian() {
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);
	setlocale(LC_ALL, "Russian");
}

// перевод цифры (символа) в число: '0'..'9', 'a'..'f'
int charToDigit(char c) {
	if (c >= '0' && c <= '9') return c - '0';
	if (c >= 'a' && c <= 'f') return c - 'a' + 10;
	if (c >= 'A' && c <= 'F') return c - 'A' + 10;
	return -1; // неверный символ
}

// перевод числа (символа) в цифру: 0..15 -> '0'..'f'
char digitToChar(int d) {
	if (d < 10) return '0' + d;
	return 'a' + (d - 10);
}

// переводим строку из системы base в десятичное число
long long toDecimal(string s, int base) {
	long long result = 0;
	for (int i = 0; i < (int)s.size(); i++) {
		int d = charToDigit(s[i]);
		result = result * base + d;
	}
	return result;
}

// переводим десятичное число в систему base
string fromDecimal(long long n, int base) {
	if (n == 0) return "0";
	string result = "";
	while (n > 0) {
		int d = n % base;
		result = digitToChar(d) + result;
		n = n / base;
	}
	return result;
}

// проверка, что число корректно для данной системы счисления
bool isValid(string s, int base) {
	for (int i = 0; i < (int)s.size(); i++) {
		int d = charToDigit(s[i]);
		if (d < 0 || d >= base) return false;
	}
	return true;
}

// арифметика
void arithmetic() {
	long long a, b;
	int op;

	cout << "\nвведите первое число: ";
	cin >> a;
	cout << "введите второе число: ";
	cin >> b;

	cout << "выберите операцию:\n";
	cout << "1 - сложение\n";
	cout << "2 - вычитание\n";
	cout << "3 - умножение\n";
	cout << "4 - деление\n";
	cout << "ваш выбор: ";
	cin >> op;

	switch (op) {
	case 1:
		cout << a << " + " << b << " = " << a + b << endl;
		break;
	case 2:
		cout << a << " - " << b << " = " << a - b << endl;
		break;
	case 3:
		cout << a << " * " << b << " = " << a * b << endl;
		break;
	case 4:
		if (b == 0) {
			cout << "на ноль делить нельзя" << endl;
		}
		else {
			cout << a << " / " << b << " = " << (double)a / b << endl;
		}
		break;
	default:
		cout << "неверная операция" << endl;
	}
}

// перевод между системами счисления
void conversion() {
	string s;
	int fromBase, toBase;

	cout << "\nвведите число: ";
	cin >> s;

	cout << "из какой системы? (2, 4, 8, 10, 16): ";
	cin >> fromBase;

	if (!isValid(s, fromBase)) {
		cout << "число не подходит для этой системы счисления" << endl;
		return;
	}

	cout << "в какую систему? (2, 4, 8, 10, 16): ";
	cin >> toBase;

	long long dec = toDecimal(s, fromBase);
	string result = fromDecimal(dec, toBase);

	cout << s << " (основание " << fromBase << ") = "
		<< result << " (основание " << toBase << ")" << endl;
}

int main() {
	enableRussian();

	int choice;

	while (true) {
		cout << "\n===== калькулятор =====\n";
		cout << "1 - арифметика (+, -, *, /)\n";
		cout << "2 - перевод систем счисления\n";
		cout << "0 - выход\n";
		cout << "ваш выбор: ";
		cin >> choice;

		if (choice == 0) {
			cout << "пока!" << endl;
			break;
		}
		else if (choice == 1) {
			arithmetic();
		}
		else if (choice == 2) {
			conversion();
		}
		else {
			cout << "нет такого пункта" << endl;
		}
	}

	return 0;
}