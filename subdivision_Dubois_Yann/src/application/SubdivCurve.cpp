#include "SubdivCurve.h"
#include <cmath>
#include <iostream>

#include "Vector3.h"
#include "Matrix4.h"

using namespace std;
using namespace p3d;

SubdivCurve::~SubdivCurve() {
}

SubdivCurve::SubdivCurve() {
  _nbIteration=1;
  _source.clear();
  _result.clear();

}


void SubdivCurve::addPoint(const p3d::Vector3 &p) {
  _source.push_back(p);
}

void SubdivCurve::point(int i,const p3d::Vector3 &p) {
  _source[i]=p;
}


void SubdivCurve::chaikinIter(const vector<Vector3> &p) {
  /* TODO : one iteration of Chaikin : input = p, output = you must set the vector _result (vector of Vector3)
   */
    _result.clear();

    for(unsigned int i = 0; i < p.size(); i++) {
      Vector3 pi1 = p[i + 1];
      if(this->isClosed() && i == p.size() - 1) pi1 = p[0];
      _result.push_back((3.0f / 4.0f) * p[i] + (1.0f / 4.0f) * pi1);

      _result.push_back((1.0f / 4.0f) * p[i] + (3.0f / 4.0f) * pi1);
    }
}

void SubdivCurve::dynLevinIter(const vector<Vector3> &p) {
  /* TODO : one iteration of DynLevin : input = p, output = you must set the vector _result (vector of Vector3)
   */
  _result.clear();

    for(unsigned int i = 0; i < p.size(); i++) {
        _result.push_back(p[i % p.size()]);
        _result.push_back(-(1.0f / 16.0f) * (p[(i + 2) % p.size()] + p[(i + 1) % p.size()]) + (9.0f / 16.0f) * (p[(i + 1) % p.size()] + p[i % p.size()]));
    }

}


void SubdivCurve::chaikin() {
  if (_source.size()<2) return;
  vector<Vector3> current;
  _result=_source;
  for(int i=0;i<_nbIteration;++i) {
    current=_result;
    chaikinIter(current);
  }
}

void SubdivCurve::dynLevin() {
  if (_source.size()<2) return;
  if (!isClosed()) return;
  vector<Vector3> current;
  _result=_source;
  for(int i=0;i<_nbIteration;++i) {
    current=_result;
    dynLevinIter(current);
  }
}


